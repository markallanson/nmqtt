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
using System.Threading;

namespace Nmqtt
{
    /// <summary>
    /// Connection handler that performs connections and disconnections to the server in a synchronous manner.
    /// </summary>
    internal sealed class SynchronousMqttConnectionHandler : MqttConnectionHandler
    {
        private AutoResetEvent connectionResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// Synchronously connect to the specific Mqtt Connection.
        /// </summary>
        /// <param name="connection"></param>
        protected override ConnectionState InternalConnect(string server, int port, MqttConnectMessage connectMessage)
        {
            // Initiate the connection
            connectionState = ConnectionState.Connecting;
            connection = MqttConnection.Connect(server, port);
            connection.DataAvailable += connection_ConnectDataAvailable;
            connection.DataAvailable += connection_MessageDataAvailable;

            // transmit the required connection message to the broker.
            SendMessage(connectMessage);

            // we're the sync connection handler so we need to wait for the brokers acknowlegement of the connections
            // TODO: Figure out how we are going to deal with sync connection timeouts, when the server doesn't respond with a ConnectAck.
            connectionResetEvent.WaitOne();
            return connectionState;
        }

        protected override ConnectionState Disconnect()
        {
            // send a disconnect message to the broker
            connectionState = ConnectionState.Disconnecting;
            SendMessage(new MqttDisconnectMessage());
            connection.DataAvailable -= connection_MessageDataAvailable;
            PerformConnectionDisconnect();
            return connectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        /// Disconnects the underlying connection object.
        /// </summary>
        private void PerformConnectionDisconnect()
        {
            // set the connection to disconnected.
            connectionState = ConnectionState.Disconnecting;
            connection.Dispose();
            connection = null;
            connectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        /// Handles the DataAvailable event of the connection control for handling connection messages.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Nmqtt.DataAvailableEventArgs"/> instance containing the event data.</param>
        void connection_ConnectDataAvailable(object sender, DataAvailableEventArgs e)
        {
            try
            {
                MqttMessage msg = MqttMessage.CreateFrom(e.MessageData);

                // Ignore any non connection based messages.
                if (msg.Header.MessageType == MqttMessageType.ConnectAck)
                {
                    connection.DataAvailable -= connection_ConnectDataAvailable;                
                    MqttConnectAckMessage ackMsg = (MqttConnectAckMessage)msg;

                    // drop the connection if our connect request has been rejected.
                    if (ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.BrokerUnavailable ||
                        ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.IdentifierRejected ||
                        ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.UnacceptedProtocolVersion)
                    {
                        // TODO: Decide on a way to let the client know why we have been rejected.
                        PerformConnectionDisconnect();
                    }
                    else
                    {
                        // initialize the keepalive to start the ping based keepalive process.
                        keepAlive = new MqttConnectionKeepAlive(this, connectMessage.VariableHeader.KeepAlive);
                        connectionState = ConnectionState.Connected;
                    }

                    connectionResetEvent.Set();
                }
            }
            catch (InvalidMessageException)
            {
                connection.DataAvailable -= connection_ConnectDataAvailable;
                PerformConnectionDisconnect();

                // not exactly, ready, but we've reached an end state so we should signal a bad connection attempt.
                connectionResetEvent.Set();
            }
        }
    }
}
