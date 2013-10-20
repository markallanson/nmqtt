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
using System.IO;
using Nmqtt.Encoding;
using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    ///     Class that contains details related to an MQTT Connect messages payload
    /// </summary>
    internal sealed class MqttConnectPayload : MqttPayload
    {
        private string clientIdentifier;
        private readonly MqttConnectVariableHeader variableHeader;
        private string username;
        private string password;

        /// <summary>
        ///     The identifier of the client that is/has sent the connet message.
        /// </summary>
        public string ClientIdentifier {
            get { return clientIdentifier; }
            set {
                if (value.Length > Constants.MaxClientIdentifierLength) {
                    throw new ClientIdentifierException(value);
                }

                clientIdentifier = value;
            }
        }

        public string WillTopic   { get; set; }
        public string WillMessage { get; set; }
        
        public string Username {
            get { return username; }
            set { username = value != null ? value.Trim() : value; }
        }

        public string Password {
            get { return password; }
            set { password = value != null ? value.Trim() : value; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectPayload" /> class.
        /// </summary>
        public MqttConnectPayload(MqttConnectVariableHeader variableHeader) {
            this.variableHeader = variableHeader;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectPayload" /> class.
        /// </summary>
        /// <param name="variableHeader">The variable header to use for the message.</param>
        /// <param name="payloadStream">The payload stream.</param>
        public MqttConnectPayload(MqttConnectVariableHeader variableHeader, Stream payloadStream) {
            this.variableHeader = variableHeader;
            ReadFrom(payloadStream);
        }

        /// <summary>
        ///     Writes the connect message payload to the supplied stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        /// <remarks>
        ///     A basic message has no Variable Header.
        /// </remarks>
        public override void WriteTo(Stream payloadStream) {
            payloadStream.WriteMqttString(ClientIdentifier);
            if (variableHeader.ConnectFlags.WillFlag) {
                payloadStream.WriteMqttString(WillTopic);
                payloadStream.WriteMqttString(WillMessage);
            }

            if (variableHeader.ConnectFlags.UsernameFlag)
            {
                payloadStream.WriteMqttString(Username);
            }

            if (variableHeader.ConnectFlags.PasswordFlag)
            {
                payloadStream.WriteMqttString(Password);
            }
        }

        /// <summary>
        ///     Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void ReadFrom(Stream payloadStream) {
            ClientIdentifier = payloadStream.ReadMqttString();

            if (this.variableHeader.ConnectFlags.WillFlag) {
                WillTopic = payloadStream.ReadMqttString();
                WillMessage = payloadStream.ReadMqttString();
            }

            if (variableHeader.ConnectFlags.UsernameFlag)
            {
                Username = payloadStream.ReadMqttString();
            }

            if (variableHeader.ConnectFlags.PasswordFlag)
            {
                Password = payloadStream.ReadMqttString();
            }
        }

        internal override int GetWriteLength() {
            int length = 0;
            var enc = new MqttEncoding();

            length += enc.GetByteCount(ClientIdentifier);

            if (this.variableHeader.ConnectFlags.WillFlag) {
                length += enc.GetByteCount(WillTopic);
                length += enc.GetByteCount(WillMessage);
            }

            if (variableHeader.ConnectFlags.UsernameFlag)
            {
                length += enc.GetByteCount(Username);
            }

            if (variableHeader.ConnectFlags.PasswordFlag)
            {
                length += enc.GetByteCount(Password);
            }

            return length;
        }

        /// <summary>
        ///     Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString()
        {
            return String.Format("Payload: ClientIdentifier={0}, WillTopic={1}, WillMessage={2}, Username={3}, Password={4}",
                ClientIdentifier, WillTopic, WillMessage, Username, Password);
        }
    }
}
