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
    ///     Implementation of an MQTT Publish Complete Message.
    /// </summary>
    internal sealed partial class MqttPublishCompleteMessage : MqttMessage
    {
        /// <summary>
        ///     Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttPublishCompleteVariableHeader VariableHeader { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttPublishCompleteMessage" /> class.
        /// </summary>
        /// <remarks>
        ///     Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttPublishCompleteMessage() {
            this.Header = new MqttHeader() {
                MessageType = MqttMessageType.PublishComplete
            };

            this.VariableHeader = new MqttPublishCompleteVariableHeader() {
                ConnectFlags = new MqttConnectFlags()
            };
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttPublishCompleteMessage" /> class.
        /// </summary>
        /// <param name="header">The header to use for the message.</param>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttPublishCompleteMessage(MqttHeader header, Stream messageStream) {
            this.Header = header;
            this.VariableHeader = new MqttPublishCompleteVariableHeader(messageStream);
        }

        /// <summary>
        ///     Writes the message to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to write the message to.</param>
        public override void WriteTo(Stream messageStream) {
            this.Header.WriteTo(VariableHeader.GetWriteLength(), messageStream);
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