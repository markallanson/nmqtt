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
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Abstract base that provides shared connection functionality to connection handler impementations.
    /// </summary>
    internal abstract class MqttConnectionHandler : IDisposable
    {
        protected MqttConnection connection;

        protected ConnectionState connectionState = ConnectionState.Disconnected;
        /// <summary>
        /// Gets the current conneciton state of the Mqtt Client.
        /// </summary>
        public ConnectionState State
        {
            get
            {
                return connectionState;
            }
        }

        /// <summary>
        /// Connect to the specific Mqtt Connection.
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="message">The connect message to use as part of the connection process.</param>
        public abstract ConnectionState Connect(string server, int port, MqttConnectMessage message);

        /// <summary>
        /// Sends a message to the broker through the current connection.
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(MqttMessage message)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                connection.Send(stream);
            }
        }

        /// <summary>
        /// Runs the disconnection process to stop communicating with a message broker.
        /// </summary>
        /// <returns></returns>
        protected abstract ConnectionState Disconnect();

        /// <summary>
        /// Closes the connection to the Mqtt message broker.
        /// </summary>
        public void Close()
        {
            if (connectionState == ConnectionState.Connecting)
            {
                // TODO: Decide what to do if the caller tries to close a connection that is in the process of being connected.
            }

            if (connectionState == ConnectionState.Connected)
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Handles the DataAvailable event of the connection control for handling non connection messages
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Nmqtt.DataAvailableEventArgs"/> instance containing the event data.</param>
        protected void connection_MessageDataAvailable(object sender, DataAvailableEventArgs e)
        {
            try
            {
                MqttMessage msg = MqttMessage.CreateFrom(e.MessageData);

                // filter out messages that are not related to standard message processing
                if (msg.Header.MessageType == MqttMessageType.Connect ||
                    msg.Header.MessageType == MqttMessageType.ConnectAck ||
                    msg.Header.MessageType == MqttMessageType.Disconnect)
                {
                    return;
                }

                // we've got a non connection based message so forward it on to the client
                if (MessageReceived != null)
                {
                    MessageReceived(this, new MessageReceivedEventArgs(msg));
                }
            }
            catch (InvalidMessageException)
            {
                // TODO: Figure out what to do if we receive a dud message during normal processing.
            }
        }

        /// <summary>
        /// Event fired by the connection handler when a message has been received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        #region IDisposable Members

        /// <summary>
        /// Cleans up the underlying raw connection to the server.
        /// </summary>
        public void Dispose()
        {
            if (connection != null)
            {
                this.Close();
                connection.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
