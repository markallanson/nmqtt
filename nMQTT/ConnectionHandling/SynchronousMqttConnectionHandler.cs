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
using System.Threading;

namespace Nmqtt
{
    /// <summary>
    ///     Connection handler that performs connections and disconnections to the hostname in a synchronous manner.
    /// </summary>
    internal sealed class SynchronousMqttConnectionHandler : MqttConnectionHandler
    {
        private readonly AutoResetEvent connectionResetEvent = new AutoResetEvent(false);
        private const int MaxConnectionAttempts = 5;

        /// <summary>
        ///     Synchronously connect to the specific Mqtt Connection.
        /// </summary>
        /// <param name="hostname">The hostname hostnameto connect to.</param>
        /// <param name="port">The port on the host to connect to.</param>
        /// <param name="connectMessage">The connection message that should be used to initiate the connection.</param>
        protected override ConnectionState InternalConnect(string hostname, int port, MqttConnectMessage connectMessage) {
            int connectionAttempts = 0;

            do {
                // Initiate the connection
                connectionState = ConnectionState.Connecting;

                connection = MqttConnection.Connect(hostname, port);
                this.RegisterForMessage(MqttMessageType.ConnectAck, ConnectAckProcessor);
                connection.DataAvailable += connection_MessageDataAvailable;

                // transmit the required connection message to the broker.
                SendMessage(connectMessage);

                // we're the sync connection handler so we need to wait for the brokers acknowlegement of the connections
                if (!connectionResetEvent.WaitOne(5000, false)) {
                    // if we don't get a response in 5 seconds, dispose the connection and rebuild it.
                    connectionState = ConnectionState.Disconnecting;
                    connection.Dispose();
                    connectionState = ConnectionState.Disconnected;
                }
            } while (connectionState != ConnectionState.Connected && ++connectionAttempts < MaxConnectionAttempts);

            // if we've failed to handshake with the broker, throw an exception.
            if (connectionState != ConnectionState.Connected) {
                throw new ConnectionException(
                    String.Format("The maximum allowed connection attempts ({0}) were exceeded. The broker " +
                                  "is not responding to the connection request message (Missing Connection Acknowledgement)",
                                  MaxConnectionAttempts),
                    connectionState);
            }

            return connectionState;
        }

        protected override ConnectionState Disconnect() {
            // send a disconnect message to the broker
            connectionState = ConnectionState.Disconnecting;
            SendMessage(new MqttDisconnectMessage());
            connection.DataAvailable -= connection_MessageDataAvailable;
            PerformConnectionDisconnect();
            return connectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        ///     Disconnects the underlying connection object.
        /// </summary>
        private void PerformConnectionDisconnect() {
            // set the connection to disconnected.
            connectionState = ConnectionState.Disconnecting;
            connection.Dispose();
            connection = null;
            connectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        ///     Processes the connect acknowledgement message.
        /// </summary>
        /// <param name="msg">The connect acknowledgement message.</param>
        private bool ConnectAckProcessor(MqttMessage msg) {
            try {
                var ackMsg = (MqttConnectAckMessage) msg;

                // drop the connection if our connect request has been rejected.
                if (ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.BrokerUnavailable ||
                    ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.IdentifierRejected ||
                    ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.UnacceptedProtocolVersion ||
                    ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.NotAuthorized ||
                    ackMsg.VariableHeader.ReturnCode == MqttConnectReturnCode.BadUsernameOrPassword)
                {
                    // TODO: Decide on a way to let the client know why we have been rejected.
                    PerformConnectionDisconnect();
                } else {
                    // initialize the keepalive to start the ping based keepalive process.
                    connectionState = ConnectionState.Connected;
                }

                connectionResetEvent.Set();
            } catch (InvalidMessageException) {
                PerformConnectionDisconnect();

                // not exactly, ready, but we've reached an end state so we should signal a bad connection attempt.
                connectionResetEvent.Set();
            }

            return true;
        }
    }
}
