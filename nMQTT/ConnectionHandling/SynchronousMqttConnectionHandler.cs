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
            this.RegisterForMessage(MqttMessageType.ConnectAck, ConnectAckProcessor);
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
        /// Processes the connect acknowledgement message.
        /// </summary>
        /// <param name="msg">The connect acknowledgement message.</param>
        private bool ConnectAckProcessor(MqttMessage msg)
        {
            try
            {
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
                    connectionState = ConnectionState.Connected;
                }

                connectionResetEvent.Set();
            }
            catch (InvalidMessageException)
            {
                PerformConnectionDisconnect();

                // not exactly, ready, but we've reached an end state so we should signal a bad connection attempt.
                connectionResetEvent.Set();
            }
            finally
            {
                // connected or disconnected now, don't need to process no more.
                // TODO: Implement one time only message registrations
                //this.UnRegisterForMessage(MqttMessageType.ConnectAck, ConnectAckProcessor);
            }

            return true;
        }
    }
}
