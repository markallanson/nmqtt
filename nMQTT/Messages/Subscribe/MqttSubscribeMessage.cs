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
    internal sealed partial class MqttSubscribeMessage : MqttMessage
    {
        /// <summary>
        ///     Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttSubscribeVariableHeader VariableHeader { get; set; }

        /// <summary>
        ///     Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttSubscribePayload Payload { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttSubscribeMessage" /> class.
        /// </summary>
        /// <remarks>
        ///     Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttSubscribeMessage() {
            this.Header = new MqttHeader().AsType(MqttMessageType.Subscribe);
            this.VariableHeader = new MqttSubscribeVariableHeader();
            this.Payload = new MqttSubscribePayload();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttSubscribeMessage" /> class.
        /// </summary>
        /// <param name="header">The header to use for the message.</param>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttSubscribeMessage(MqttHeader header, Stream messageStream) {
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
            this.VariableHeader = new MqttSubscribeVariableHeader(messageStream);
            this.Payload = new MqttSubscribePayload(Header, VariableHeader, messageStream);
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