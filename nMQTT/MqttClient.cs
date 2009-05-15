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

//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// A client class for interacting with MQTT Data Packets
    /// </summary>
    public sealed class MqttClient : IDisposable
    {
        private string server;
        /// <summary>
        /// The remote server that this client will connect to.
        /// </summary>
        public string Server
        {
            get
            {
                return server;
            }
        }

        private int port;
        /// <summary>
        /// The port on the remote server that this client will connect to.
        /// </summary>
        public int Port
        {
            get
            {
                return port;
            }
        }

        private ConnectionState connectionState = ConnectionState.Disconnected;
        /// <summary>
        /// Gets the current conneciton state of the Mqtt Client.
        /// </summary>
        public ConnectionState ConnectionState
        {
            get
            {
                return connectionState;
            }
        }

        /// <summary>
        /// The raw connection to the Mqtt Broker.
        /// </summary>
        private MqttConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttClient"/> class using the default Mqtt Port.
        /// </summary>
        /// <param name="server">The server hostname to connect to.</param>
        /// <param name="clientIdentifier">The client identifier to use to connect with.</param>
        public MqttClient(string server, string clientIdentifier)
            : this(server, Constants.DefaultMqttPort, clientIdentifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttClient"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        /// <param name="clientIdentifier">The ID that the broker can use to identify the client.</param>
        public MqttClient(string server, int port, string clientIdentifier)
        {
            this.server = server;
            this.port = port;

            connection = MqttConnection.Connect(server, port);
            connection.DataAvailable += new EventHandler<DataAvailableEventArgs>(DataAvailableEventHandler);
            connectionState = ConnectionState.Connected;

            MqttConnectMessage connectMsg = new MqttConnectMessage()
                .WithClientIdentifier(clientIdentifier)
                .KeepAliveFor(500)
                .StartClean();

            SendMessage(connectMsg);
        }

        /// <summary>
        /// The primary message processor that deals with incoming messages from thr Mqtt Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataAvailableEventHandler(object sender, DataAvailableEventArgs e)
        {
        //    MqttMessage message = MqttMessage.CreateFrom(e.DataStream);
        }

        /// <summary>
        /// Sends a message to the broker.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendMessage(MqttMessage message)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                connection.Send(stream);
            }
        }

        /// <summary>
        /// Closes the MQTT Client.
        /// </summary>
        public void Close()
        {
            if (connectionState == ConnectionState.Connected && connection != null)
            {
                // send a disconnect message to the broker and dispose the connection object.
                SendMessage(new MqttDisconnectMessage());
                connection.Dispose();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
