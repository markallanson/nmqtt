/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections.ObjectModel;

using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    internal class MqttConnection : IDisposable
    {
        private TcpClient tcpClient;
        private BufferedStream networkStream;

        private byte[] headerByte = new byte[1];

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnection"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        private MqttConnection(string server, int port)
        {
            // connect and save off the stream.
            tcpClient = new TcpClient(server, port);
            networkStream = new BufferedStream(tcpClient.GetStream());

            // initiate a read for the next 2 bytesm which should be header bytes.
            networkStream.BeginRead(headerByte, 0, 1, new AsyncCallback(ReadComplete), networkStream);
        }

        /// <summary>
        /// Initiate a new connection to a message broker
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public static MqttConnection Connect(string server, int port)
        {
            return new MqttConnection(server, port);
        }

        /// <summary>
        /// Disconnects from the message broker
        /// </summary>
        private void Disconnect()
        {
            tcpClient.Close();
        }

        /// <summary>
        /// Sends the message in the stream to the broker.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(Stream message)
        {
            byte[] messageBytes = new byte[message.Length];
            message.Read(messageBytes, 0, (int)message.Length);
            Send(messageBytes);
        }

        /// <summary>
        /// Sends the message contained in the byte array to the broker.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(byte[] message)
        {
            if (networkStream != null)
            {
                networkStream.Write(message, 0, message.Length);
                networkStream.Flush();
            }
            else
            {
                // todo: throw an exception if there is no network stream.
            }
        }

        /// <summary>
        /// Callback for when data is available for reading from the underlying stream.
        /// </summary>
        /// <param name="state">The state.</param>
        private void ReadComplete(IAsyncResult asyncResult)
        {
            var bytesRead = 0;
            Stream dataStream = (Stream)asyncResult.AsyncState;

            try
            {
                bytesRead = dataStream.EndRead(asyncResult);
            }
            catch (IOException)
            {
                // TODO: Implement handling of dropped connections from the other end.
                return;
            }

            if (bytesRead < 1)
            {
                // ignore the read, try again
            }

            Collection<byte> messageBytes = new Collection<byte>();
            messageBytes.Add(headerByte[0]);

            Collection<byte> lengthBytes = MqttHeader.ReadLengthBytes(dataStream);
            int length = MqttHeader.CalculateLength(lengthBytes);
            messageBytes.AddRange(lengthBytes);

            // we've got the bytes that make up the header, inc the size, read the .
            var remainingMessage = new byte[length];
            int messageBytesRead = dataStream.Read(remainingMessage, 0, length);
            if (messageBytesRead < length)
            {
                // we haven't got all the message, need to figure oput what to do.
            }
            messageBytes.AddRange(remainingMessage);

            FireDataAvailableEvent(messageBytes);
        }

        private void FireDataAvailableEvent(Collection<byte> messageBytes)
        {
            DataAvailable(this, new DataAvailableEventArgs(messageBytes));
        }

        /// <summary>
        /// Occurs when Data is available for processing from the underlying network stream.
        /// </summary>
        public event EventHandler<DataAvailableEventArgs> DataAvailable;

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            networkStream.Dispose();
            Disconnect();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
