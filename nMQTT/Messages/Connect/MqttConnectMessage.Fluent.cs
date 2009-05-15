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

namespace Nmqtt
{
    /// <summary>
    /// Implementation of an Mqtt Connect Message. Used to initiate a connection to an RSMB
    /// </summary>
    public sealed partial class MqttConnectMessage
    {
        /// <summary>
        /// Sets the name of the protocol to use.
        /// </summary>
        /// <param name="protocolName">Name of the protocol.</param>
        /// <returns></returns>
        public MqttConnectMessage WithProtocolName(string protocolName)
        {
            this.VariableHeader.ProtocolName = protocolName;
            return this;
        }

        /// <summary>
        /// Sets the protocol version. (Defaults to v3, the only protcol version supported)
        /// </summary>
        /// <param name="protocolVersion">The protocol version.</param>
        /// <returns></returns>
        public MqttConnectMessage WithProtocolVersion(byte protocolVersion)
        {
            this.VariableHeader.ProtocolVersion = protocolVersion;
            return this;
        }

        /// <summary>
        /// Sets the startClean flag so that the broker drops any messages that were previously destined for us.
        /// </summary>
        /// <returns></returns>
        public MqttConnectMessage StartClean()
        {
            this.VariableHeader.ConnectFlags.CleanStart = true;
            return this;
        }

        public MqttConnectMessage KeepAliveFor(short keepAliveSeconds)
        {
            this.VariableHeader.KeepAlive = keepAliveSeconds;
            return this;
        }

        /// <summary>
        /// Sets the Will flag of the variable header
        /// </summary>
        /// <returns></returns>
        public MqttConnectMessage Will()
        {
            this.VariableHeader.ConnectFlags.WillFlag = true;
            return this;
        }

        /// <summary>
        /// Sets the WillQos of the connect flag.
        /// </summary>
        /// <param name="qos">The qos.</param>
        /// <returns></returns>
        public MqttConnectMessage WithWillQos(MqttQos qos)
        {
            this.VariableHeader.ConnectFlags.WillQos = qos;
            return this;
        }

        /// <summary>
        /// Sets the WillRetain flag of the Connection Flags
        /// </summary>
        /// <returns></returns>
        public MqttConnectMessage WithWillRetain()
        {
            this.VariableHeader.ConnectFlags.WillRetain = true;
            return this;
        }

        /// <summary>
        /// Sets the client identifier of the message.
        /// </summary>
        /// <param name="clientID">The client ID.</param>
        /// <returns></returns>
        public MqttConnectMessage WithClientIdentifier(string clientIdentifier)
        {
            this.Payload.ClientIdentifier = clientIdentifier;
            return this;
        }

        /// <summary>
        /// Sets the will message.
        /// </summary>
        /// <param name="willMessage">The will message.</param>
        /// <returns></returns>
        public MqttConnectMessage WithWillMessage(string willMessage)
        {
            this.Payload.WillMessage = willMessage;
            return this;
        }

        /// <summary>
        /// Sets the Will Topic
        /// </summary>
        /// <param name="willTopic">The Will Topic.</param>
        /// <returns></returns>
        public MqttConnectMessage WithWillTopic(string willTopic)
        {
            this.Payload.WillTopic = willTopic;
            return this;
        }
    }
}
