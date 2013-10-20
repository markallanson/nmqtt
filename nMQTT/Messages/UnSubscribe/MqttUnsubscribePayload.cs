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
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Nmqtt.Encoding;
using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    ///     Class that contains details related to an MQTT Unsubscribe messages payload
    /// </summary>
    internal sealed class MqttUnsubscribePayload : MqttPayload
    {
        private readonly MqttVariableHeader variableHeader;
        private readonly MqttHeader header;

        private readonly Collection<string> subscriptions = new Collection<string>();

        /// <summary>
        ///     The collection of subscriptions, Key is the topic, Value is the qos
        /// </summary>
        public Collection<string> Subscriptions {
            get { return subscriptions; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttUnsubscribePayload" /> class.
        /// </summary>
        public MqttUnsubscribePayload() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttUnsubscribePayload" /> class.
        /// </summary>
        /// <param name="payloadStream">The payload stream.</param>
        /// <param name="header">The header to use for the message.</param>
        /// <param name="variableHeader">The variable header to use for the message.</param>
        public MqttUnsubscribePayload(MqttHeader header, MqttUnsubscribeVariableHeader variableHeader,
                                      Stream payloadStream) {
            this.header = header;
            this.variableHeader = variableHeader;

            ReadFrom(payloadStream);
        }

        /// <summary>
        ///     Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void ReadFrom(Stream payloadStream) {
            int payloadBytesRead = 0;
            int payloadLength = header.MessageSize - variableHeader.Length;

            // read all the topics and qos subscriptions from the message payload
            while (payloadBytesRead < payloadLength) {
                string topic = payloadStream.ReadMqttString();

                payloadBytesRead += topic.Length + 2; // +2 = Mqtt string length bytes

                AddSubscription(topic);
            }
        }

        /// <summary>
        ///     Writes the payload to the supplied stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void WriteTo(Stream payloadStream) {
            foreach (var topic in subscriptions) {
                payloadStream.WriteMqttString(topic);
            }
        }

        /// <summary>
        ///     Gets the length of the payload in bytes when written to a stream.
        /// </summary>
        /// <returns>The length of the payload in bytes.</returns>
        internal override int GetWriteLength() {
            int length = 0;
            var enc = new MqttEncoding();

            foreach (var topic in subscriptions) {
                length += enc.GetByteCount(topic);
            }

            return length;
        }

        /// <summary>
        ///     Adds a new subscription to the collection of subscriptions.
        /// </summary>
        /// <param name="topic">The topic to Unsubscribe from.</param>
        public void AddSubscription(string topic) {
            if (!Subscriptions.Contains(topic)) {
                Subscriptions.Add(topic);
            }
        }

        /// <summary>
        ///     Clears the subscriptions.
        /// </summary>
        public void ClearSubscriptions() {
            Subscriptions.Clear();
        }

        /// <summary>
        ///     Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(String.Format("Payload: Subscription [{0}]", subscriptions.Count));

            foreach (var topic in Subscriptions) {
                sb.AppendLine(String.Format("{{ Topic={0} }}", topic));
            }

            return sb.ToString();
        }
    }
}