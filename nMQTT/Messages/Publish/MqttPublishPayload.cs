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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    ///     Class that contains details related to an MQTT Connect messages payload
    /// </summary>
    internal sealed class MqttPublishPayload : MqttPayload
    {
        private readonly MqttHeader header;
        private readonly MqttPublishVariableHeader variableHeader;


        private Collection<byte> message;

        /// <summary>
        ///     The message that forms the payload of the publish message.
        /// </summary>
        public Collection<byte> Message {
            get { return message; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectPayload" /> class.
        /// </summary>
        public MqttPublishPayload() {
            this.message = new Collection<byte>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectPayload" /> class.
        /// </summary>
        /// <param name="header">The header of the message being process.</param>
        /// <param name="variableHeader">The variable header of the message being processed.</param>
        /// <param name="payloadStream">The payload stream.</param>
        public MqttPublishPayload(MqttHeader header, MqttPublishVariableHeader variableHeader, Stream payloadStream) {
            this.header = header;
            this.variableHeader = variableHeader;
            ReadFrom(payloadStream);
        }

        /// <summary>
        ///     Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void ReadFrom(Stream payloadStream) {
            // The payload of the publish message is not a string, just a binary chunk of bytes.
            // The length of the bytes is the length specified in the header, minus any bytes 
            // spent in the variable header.

            var messageBytes = new byte[header.MessageSize - variableHeader.Length];
            int messageBytesRead = payloadStream.Read(messageBytes, 0, messageBytes.Length);
            message = new Collection<byte>(messageBytes);

            // Throw out an exception we don't have enough bytes in the underlying stream.
            if (messageBytesRead < Message.Count) {
                throw new InvalidPayloadSizeException(
                    String.Format(
                        "The length of data in the payload ({0}) did not match the expected payload size ({1}).",
                        messageBytesRead, Message.Count));
            }
        }


        /// <summary>
        ///     Writes the payload to the supplied stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        /// <remarks>
        ///     A basic message has no Variable Header.
        /// </remarks>
        public override void WriteTo(Stream payloadStream) {
            payloadStream.Write(Message.ToArray<byte>(), 0, (int) Message.Count);
        }

        /// <summary>
        ///     Gets the length of the payload in bytes when written to a stream.
        /// </summary>
        /// <returns>The length of the payload in bytes.</returns>
        internal override int GetWriteLength() {
            return Message.Count;
        }


        /// <summary>
        ///     Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString() {
            return String.Format("Payload: {0} bytes={1}", Message == null ? -1 : Message.Count, BytesToString(Message));
        }

        /// <summary>
        ///     Converts an array of bytes to a byte string.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <returns>The message as an array of bytes</returns>
        private static string BytesToString(IEnumerable<byte> Message) {
            var sb = new StringBuilder();
            foreach (var b in Message) {
                sb.Append('<');
                sb.Append(b);
                sb.Append('>');
            }
            return sb.ToString();
        }
    }
}