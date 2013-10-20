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
    internal sealed partial class MqttUnsubscribeMessage : MqttMessage
    {
        /// <summary>
        ///     Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttUnsubscribeVariableHeader VariableHeader { get; set; }

        /// <summary>
        ///     Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttUnsubscribePayload Payload { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttUnsubscribeMessage" /> class.
        /// </summary>
        /// <remarks>
        ///     Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttUnsubscribeMessage() {
            this.Header = new MqttHeader().AsType(MqttMessageType.Unsubscribe);
            this.VariableHeader = new MqttUnsubscribeVariableHeader();
            this.Payload = new MqttUnsubscribePayload();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttUnsubscribeMessage" /> class.
        /// </summary>
        /// <param name="header">The header to use for the message.</param>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttUnsubscribeMessage(MqttHeader header, Stream messageStream) {
            this.Header = header;
            ReadFrom(messageStream);
        }

        /// <summary>
        ///     Writes the message to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to write the message to.</param>
        public override void WriteTo(Stream messageStream) {
            this.Header.WriteTo(this.VariableHeader.GetWriteLength() + this.Payload.GetWriteLength(),
                                messageStream);
            this.VariableHeader.WriteTo(messageStream);
            this.Payload.WriteTo(messageStream);
        }

        /// <summary>
        ///     Reads a message from the supplied stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        public override void ReadFrom(Stream messageStream) {
            this.VariableHeader = new MqttUnsubscribeVariableHeader(messageStream);
            this.Payload = new MqttUnsubscribePayload(Header, VariableHeader, messageStream);
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
            sb.AppendLine(Payload.ToString());

            return sb.ToString();
        }
    }
}