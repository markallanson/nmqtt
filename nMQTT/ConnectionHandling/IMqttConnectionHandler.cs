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

namespace Nmqtt
{
    internal interface IMqttConnectionHandler : IDisposable
    {
        /// <summary>
        /// Closes a connection.
        /// </summary>
        void Close();

        /// <summary>
        /// Connects to a message broker
        /// </summary>
        /// <param name="server">The broker servr to connect to</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="message">The connect message to use to initiate the connection.</param>
        /// <returns></returns>
        ConnectionState Connect(string server, int port, MqttConnectMessage message);
        
        /// <summary>
        /// Register the specified callback to receive messages of a specific type.
        /// </summary>
        /// <param name="msgType">The type of message that the callback should be sent.</param>
        /// <param name="msgProcessorCallback">The callback function that will accept the message type.</param>
        void RegisterForMessage(MqttMessageType msgType, Func<MqttMessage, bool> msgProcessorCallback);

        /// <summary>
        /// Sends a message to a message broker.
        /// </summary>
        /// <param name="message">The message to send to the broker.</param>
        void SendMessage(MqttMessage message);

        /// <summary>
        /// Gets the current connection state.
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// Unregisters the specified callbacks so it not longer receives messages of the specified type.
        /// </summary>
        /// <param name="msgType">The message type the callback currently receives.</param>
        /// <param name="msgProcessorCallback">The callback to unregister.</param>
        void UnRegisterForMessage(MqttMessageType msgType, Func<MqttMessage, bool> msgProcessorCallback);
    }
}
