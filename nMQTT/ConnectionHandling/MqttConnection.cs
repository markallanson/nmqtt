/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Common.Logging;

namespace Nmqtt
{
    internal class MqttConnection : IDisposable
    {
        private static readonly ILog   Log = LogManager.GetCurrentClassLogger(); 

        /// <summary>
        ///     The TcpClient that maintains the connection to the MQTT broker.
        /// </summary>
        private readonly TcpClient     tcpClient;

        /// <summary>
        ///     Sync lock object to ensure that only a single message is sent through the connection handler at once.
        /// </summary>
        private readonly object        sendPadlock = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnection" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        private MqttConnection(string server, int port) {
            try {
                Log.Debug(m => m("Connecting to message broker running on {0}:{1}", server, port));
                // connect and save off the stream.
                tcpClient = new TcpClient(server, port);
            } catch (SocketException ex) {
                String message = String.Format("The connection to the message broker {0}:{1} could not be made.", server, port);
                Log.Error(message, ex);
                throw new ConnectionException(message, ConnectionState.Faulted, ex);
            }

            // get and stash the network stream
            var readWrapper = new ReadWrapper(tcpClient.GetStream());
            readWrapper.Stream.BeginRead(readWrapper.Buffer, 0, readWrapper.NextReadSize, ReadHeaderComplete, readWrapper);
        }

        /// <summary>
        ///     Initiate a new connection to a message broker
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public static MqttConnection Connect(string server, int port) {
            return new MqttConnection(server, port);
        }

        /// <summary>
        ///     Disconnects from the message broker
        /// </summary>
        private void Disconnect() {
            tcpClient.Close();
        }

        /// <summary>
        ///     Sends the message in the stream to the broker.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void Send(Stream message) {
            var messageBytes = new byte[message.Length];
            message.Read(messageBytes, 0, (int) message.Length);
            Send(messageBytes);
        }

        /// <summary>
        ///     Sends the message contained in the byte array to the broker.
        /// </summary>
        /// <param name="message">The message to send.</param>
        private void Send(byte[] message) {
            Log.Info(m => m("Sending message of {0} bytes to connected broker", message.Length));
            // ensure only a single thread gets through to do wire ops at once.
            lock (sendPadlock) {
                var stream = tcpClient.GetStream();
                stream.Write(message, 0, message.Length);
                stream.Flush();
            }
        }

        /// <summary>
        ///     Callback for when data has been read from the underlying network stream.
        /// </summary>
        /// <param name="asyncResult">The async result from the read.</param>
        private void ReadHeaderComplete(IAsyncResult asyncResult) {
            var readWrapper = (ReadWrapper) asyncResult.AsyncState;

            try {
                var bytesRead = readWrapper.Stream.EndRead(asyncResult);
                if (bytesRead == 0) {
                    // Nothing read, we will just try another read from the stream.
                    Log.Debug("Async network stream read returned 0 bytes, continuing to search for header.");
                    readWrapper.ReadState = ConnectionReadState.Header;
                } else if (tcpClient.Connected && readWrapper.Stream.CanRead) {
                    if (readWrapper.ReadState == ConnectionReadState.Header && readWrapper.Stream.DataAvailable) {
                        Log.Info("Reading message arriving on the wire.");

                        readWrapper.MessageBytes.Add(readWrapper.Buffer[0]);

                        var lengthBytes = MqttHeader.ReadLengthBytes(readWrapper.Stream);
                        var remainingLength = MqttHeader.CalculateLength(lengthBytes);

                        // update the read wrapper with the header bytes, and a resized read buffer
                        // to capture the remaining length.
                        readWrapper.MessageBytes.AddRange(lengthBytes);

                        // no content, so yield the message early, else transition to reading the content.
                        if (remainingLength == 0) {
                            Log.Debug("Message receipt complete. Has empty content length so handing off now.");
                            FireDataAvailableEvent(readWrapper.MessageBytes);
                        } else {
                            // total bytes of content is the remaining length plus the header.
                            readWrapper.TotalBytes = remainingLength + readWrapper.MessageBytes.Count;
                            readWrapper.RecalculateNextReadSize();
                            readWrapper.ReadState = ConnectionReadState.Content;
                        }
                    } else if (readWrapper.ReadState == ConnectionReadState.Content) {
                        // stash what we've read.
                        readWrapper.MessageBytes.AddRange(readWrapper.Buffer.Take(bytesRead));
                        Log.Debug(m => m("Message Content read {0:n0} of {1:n0} expected remaining bytes.", bytesRead, readWrapper.TotalBytes));

                        // if we haven't yet read all of the message repeat the read otherwise if
                        // we're finished process the message and switch back to waiting for the next header.
                        if (readWrapper.IsReadComplete) {
                            // reset the read buffer to accommodate the remaining length (last - what was read)
                            readWrapper.RecalculateNextReadSize();
                        } else {
                            Log.Debug(m => m("Message receipt complete ({0:n0} total bytes including all headers), handing off to handlers.", readWrapper.MessageBytes.Count));
                            readWrapper.ReadState = ConnectionReadState.Header;                            
                            FireDataAvailableEvent(readWrapper.MessageBytes);
                        }
                    }

                    // if we've switched to reading a header then recreate the read dwrapper for the next message
                    if (readWrapper.ReadState == ConnectionReadState.Header) {
                        readWrapper = new ReadWrapper(readWrapper.Stream);
                    }
                    // we can still read etc
                    // initiate a read for the next set of bytes which will be the header bytes so long as
                    // we're still connected to the underlying client
                    readWrapper.Stream.BeginRead(readWrapper.Buffer, 0, readWrapper.NextReadSize, ReadHeaderComplete, readWrapper);
                }
            } catch (IOException ex) {
                Log.Debug("Error occurred during async read from broker network stream. Initiating broker disconnect", ex);

                // close the underlying connection
                this.Disconnect();

                if (ConnectionDropped != null) {
                    ConnectionDropped(this, new ConnectionDroppedEventArgs(ex));
                }
            }
        }

        /// <summary>
        ///     Raises the DataAvailable event, passing the raw message bytes to all subscribers.
        /// </summary>
        /// <param name="messageBytes">The raw content of the message received.</param>
        private void FireDataAvailableEvent(List<byte> messageBytes) {
            Log.Debug("Dispatching completed message to handlers.");
            if (DataAvailable != null) {
                DataAvailable(this, new DataAvailableEventArgs(messageBytes));
            }
        }

        /// <summary>
        ///     Occurs when Data is available for processing from the underlying network stream.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        /// <summary>
        ///     Occurs when the connection to the remote server drops unexpectedly.
        /// </summary>
        public event EventHandler<ConnectionDroppedEventArgs> ConnectionDropped;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Controls the read state used during async reads.
        /// </summary>
        private enum ConnectionReadState {
            /// <summary>
            ///     Reading a message header.
            /// </summary>
            Header,

            /// <summary>
            ///     Reading message content. 
            /// </summary>
            Content
        }

        /// <summary>
        ///     State and logic used to read from the underlying network stream.
        /// </summary>
        private struct ReadWrapper
        {
            /// <summary>
            /// The read buffer size from the network
            /// </summary>
            private const int             BufferSize = 1 << 17; // 128KB read buffer

            /// <summary>
            ///     The total bytes expected to be read from from the header of content
            /// </summary>
            public int                    TotalBytes;

            /// <summary>
            ///     The bytes associated with the message being read.
            /// </summary>
            public readonly List<byte>    MessageBytes;

            /// <summary>
            ///     The network stream being read.
            /// </summary>
            public readonly NetworkStream Stream;

            /// <summary>
            ///     The amount of content to read during the next read.
            /// </summary>
            public int                    NextReadSize;

            /// <summary>
            ///     The buffer the last stream read wrote into.
            /// </summary>
            public readonly byte[]        Buffer;

            /// <summary>
            ///     What is the connection currently reading.
            /// </summary>
            public ConnectionReadState    ReadState;

            /// <summary>
            /// A boolean that indicates whether the message read is complete 
            /// </summary>
            public bool IsReadComplete {
                get { return MessageBytes.Count < TotalBytes; }
            }

            /// <summary>
            ///     Creates a new ReadWrapper that wraps the state used to read a message from a stream.
            /// </summary>
            /// <param name="stream">The stream being read.</param>
            public ReadWrapper(NetworkStream stream) {
                this.ReadState    = ConnectionReadState.Header;
                this.Buffer       = new byte[BufferSize];
                this.MessageBytes = new List<byte>(); 
                this.TotalBytes   = 1; // default to header read size.
                this.NextReadSize = this.TotalBytes;
                this.Stream       = stream;
            }

            /// <summary>
            ///     Recalculates the number of best to read given the expected total size and the amount read so far.
            /// </summary>
            public void RecalculateNextReadSize() {
                if (TotalBytes == 0) {
                    throw new InvalidOperationException("Total ReadBytes is 0, cannot calculate next read size.");
                }
                // next read size is the buffersize if we have more than buffer size left to read, 
                // otherwise it's the amount left to read in the message.
                var remainingBytes = TotalBytes - MessageBytes.Count;
                this.NextReadSize = remainingBytes > BufferSize
                                        ? BufferSize
                                        : remainingBytes;
            }
        }
    }
}