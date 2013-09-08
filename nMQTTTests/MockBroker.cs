using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;

using Nmqtt;
using Nmqtt.ExtensionMethods;

namespace NmqttTests
{
    /// <summary>
    /// Mocks a broker, such as the RSMB, so that we can test the MqttConnection class, and some bits of the
    /// connection handlers that are difficult to test otherwise.
    /// </summary>
    internal class MockBroker : IDisposable
    {
        int brokerPort = 1883;
        TcpListener listener = null;

        TcpClient client = null;
        NetworkStream networkStream = null;
        byte[] headerBytes = new byte[1];

        Action<byte[]> messageHandler;

        public MockBroker()
        {
            listener = new TcpListener(Dns.GetHostAddresses("localhost")[0], brokerPort);
            listener.Start();

            listener.BeginAcceptTcpClient(ConnectAccept, null);
        }

        private void ConnectAccept(IAsyncResult connectResult)
        {
            client = listener.EndAcceptTcpClient(connectResult);
            networkStream = client.GetStream();
            networkStream.BeginRead(headerBytes, 0, 1, DataArrivedOnConnection, null);
        }

        private void DataArrivedOnConnection(IAsyncResult result)
        {
            int bytesRead = networkStream.EndRead(result);

            Collection<byte> messageBytes = new Collection<byte>();
            messageBytes.Add(headerBytes[0]);

            Collection<byte> lengthBytes = MqttHeader.ReadLengthBytes(networkStream);
            int length = MqttHeader.CalculateLength(lengthBytes);
            messageBytes.AddRange<byte>(lengthBytes);

            // we've got the bytes that make up the header, inc the size, read the .
            var remainingMessage = new byte[length];
            int messageBytesRead = networkStream.Read(remainingMessage, 0, length);
            if (messageBytesRead < length)
            {
                // we haven't got all the message, need to figure oput what to do.
            }
            messageBytes.AddRange<byte>(remainingMessage);

            networkStream.BeginRead(headerBytes, 0, 1, DataArrivedOnConnection, null);

        }

        /// <summary>
        /// Sets a function that will be passed the next message received by the faked out broker.
        /// </summary>
        /// <param name="handler"></param>
        public void SetMessageHandler(Action<byte[]> handler)
        {
            messageHandler = handler;
        }

        /// <summary>
        /// Sends the message to the client connected to the broker.
        /// </summary>
        /// <param name="msg">The Mqtt Message.</param>
        public void SendMessage(MqttMessage msg)
        {
            msg.WriteTo(networkStream);
            networkStream.Flush();
        }

        #region IDisposable Members

        public void Dispose()
        {
            listener.Stop();
            GC.SuppressFinalize(this);
        }

        #endregion

        internal void SendMessage(MqttConnectAckMessage ack)
        {
            throw new NotImplementedException();
        }
    }
}
