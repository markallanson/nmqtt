/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Sockets;
using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    internal class MqttConnection : IDisposable
    {
        private readonly TcpClient tcpClient;
        private readonly NetworkStream networkStream;

        private readonly byte[] headerByte = new byte[1];

        /// <summary>
        ///     Sync lock object to ensure that only a single message is sent through the connection handler at once.
        /// </summary>
        protected object sendPadlock = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnection" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        private MqttConnection(string server, int port) {
            try {
                // connect and save off the stream.
                tcpClient = new TcpClient(server, port);
            } catch (SocketException ex) {
                throw new ConnectionException(
                    String.Format("The connection to the message broker {0}:{1} could not be made.", server, port),
                    ConnectionState.Faulted, ex);
            }

            networkStream = tcpClient.GetStream();

            // initiate a read for the next byte which will be the header bytes
            networkStream.BeginRead(headerByte, 0, 1, ReadComplete, networkStream);
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
            networkStream.Close();
            tcpClient.Close();
        }

        /// <summary>
        ///     Sends the message in the stream to the broker.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(Stream message) {
            var messageBytes = new byte[message.Length];
            message.Read(messageBytes, 0, (int) message.Length);
            Send(messageBytes);
        }

        /// <summary>
        ///     Sends the message contained in the byte array to the broker.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(byte[] message) {
            // ensure only a single thread gets through to do wire ops at once.
            lock (sendPadlock) {
                if (networkStream != null) {
                    networkStream.Write(message, 0, message.Length);
                    networkStream.Flush();
                }
            }
        }

        /// <summary>
        ///     Callback for when data is available for reading from the underlying stream.
        /// </summary>
        /// <param name="asyncResult">The async result from the read.</param>
        private void ReadComplete(IAsyncResult asyncResult) {
            int bytesRead;
            var dataStream = (NetworkStream) asyncResult.AsyncState;

            try {
                if (tcpClient.Connected && dataStream.CanRead && dataStream.DataAvailable) {
                    bytesRead = dataStream.EndRead(asyncResult);

                    if (bytesRead == 1) {
                        var messageBytes = new Collection<byte>();
                        messageBytes.Add(headerByte[0]);

                        Collection<byte> lengthBytes = MqttHeader.ReadLengthBytes(dataStream);
                        int length = MqttHeader.CalculateLength(lengthBytes);
                        messageBytes.AddRange(lengthBytes);

                        // we've got the bytes that make up the header, inc the size, read the .
                        var remainingMessage = new byte[length];
                        int messageBytesRead = dataStream.Read(remainingMessage, 0, length);
                        if (messageBytesRead < length) {
                            // we haven't got all the message, need to figure oput what to do.
                        }
                        messageBytes.AddRange(remainingMessage);

                        FireDataAvailableEvent(messageBytes);
                    }
                }

                // initiate a read for the next byte which will be the header bytes so long as
                // we're still connected to the underlying client
                if (tcpClient.Connected && networkStream.CanRead) {
                    dataStream.BeginRead(headerByte, 0, 1, ReadComplete, dataStream);
                }
            } catch (IOException ex) {
                // close the underlying connection
                this.Disconnect();

                if (ConnectionDropped != null) {
                    ConnectionDropped(this, new ConnectionDroppedEventArgs(ex));
                }
            }
        }

        private void FireDataAvailableEvent(Collection<byte> messageBytes) {
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

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            networkStream.Dispose();
            Disconnect();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}