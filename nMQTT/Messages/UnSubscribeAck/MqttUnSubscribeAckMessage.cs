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
    /// Implementation of an MQTT Unsubscribe ACK Message, used to acknowledge that an unsubscribe message has been processed.
    /// </summary>
    public sealed class MqttUnsubscribeAckMessage : MqttMessage
    {
        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttUnsubscribeAckVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttPublishMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttUnsubscribeAckMessage()
        {
            this.Header = new MqttHeader()
            {
                MessageType = MqttMessageType.UnsubscribeAck
            };

            this.VariableHeader = new MqttUnsubscribeAckVariableHeader()
            {
                ConnectFlags = new MqttConnectFlags()
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectMessage"/> class.
        /// </summary>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttUnsubscribeAckMessage(MqttHeader header, Stream messageStream)
        {
            this.Header = header;
            this.VariableHeader = new MqttUnsubscribeAckVariableHeader(messageStream);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(base.ToString());
            sb.AppendLine(VariableHeader.ToString());

            return sb.ToString();
        }
    }
}
