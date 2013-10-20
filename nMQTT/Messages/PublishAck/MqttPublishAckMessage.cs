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

using System.IO;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    ///     Implementation of an MQTT Publish Acknowledgement Message, used to ACK a publish message that has it's QOS set to AtLeast or Exactly Once.
    /// </summary>
    internal sealed partial class MqttPublishAckMessage : MqttMessage
    {
        /// <summary>
        ///     Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttPublishAckVariableHeader VariableHeader { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttPublishMessage" /> class.
        /// </summary>
        /// <remarks>
        ///     Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttPublishAckMessage() {
            this.Header = new MqttHeader() {
                MessageType = MqttMessageType.PublishAck
            };

            this.VariableHeader = new MqttPublishAckVariableHeader() {
                ConnectFlags = new MqttConnectFlags()
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectMessage" /> class.
        /// </summary>
        /// <param name="header">The header to use for the message.</param>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttPublishAckMessage(MqttHeader header, Stream messageStream) {
            this.Header = header;
            this.VariableHeader = new MqttPublishAckVariableHeader(messageStream);
        }

        public override void WriteTo(Stream messageStream) {
            this.Header.WriteTo(this.VariableHeader.GetWriteLength(), messageStream);
            this.VariableHeader.WriteTo(messageStream);
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString() {
            var sb = new StringBuilder();

            sb.Append(base.ToString());
            sb.AppendLine(VariableHeader.ToString());

            return sb.ToString();
        }
    }
}