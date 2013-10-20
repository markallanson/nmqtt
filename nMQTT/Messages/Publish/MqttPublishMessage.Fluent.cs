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

using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    ///     Implementation of an MQTT Publish Message, used for publishing telemetry data along a live MQTT stream.
    /// </summary>
    internal sealed partial class MqttPublishMessage
    {
        /// <summary>
        ///     Sets the topic to publish data to.
        /// </summary>
        /// <param name="topicName">The name of the topic to publish.</param>
        /// <returns>The updated instance of the message with the topic name set.</returns>
        public MqttPublishMessage ToTopic(string topicName) {
            this.VariableHeader.TopicName = topicName;
            return this;
        }

        /// <summary>
        ///     Appends data to publish to the end of the current message payload.
        /// </summary>
        /// <param name="data">The data to append to the end of the published data</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttPublishMessage PublishData(byte[] data) {
            this.Payload.Message.AddRange(new System.Collections.ObjectModel.Collection<byte>(data));
            return this;
        }

        /// <summary>
        ///     Sets the message identifier of the message.
        /// </summary>
        /// <param name="messageIdentifier">The ID of the message.</param>
        /// <returns>An updated instance of the message.</returns>
        public MqttPublishMessage WithMessageIdentifier(short messageIdentifier) {
            this.VariableHeader.MessageIdentifier = messageIdentifier;
            return this;
        }

        /// <summary>
        ///     Sets the Qos of the published message.
        /// </summary>
        /// <param name="qos">The qos to set.</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttPublishMessage WithQos(MqttQos qos) {
            this.Header.WithQos(qos);
            return this;
        }

        /// <summary>
        ///     Removes the current published data.
        /// </summary>
        /// <returns>The updated instance of the message with published data cleared</returns>
        public MqttPublishMessage ClearPublishData() {
            this.Payload.Message.Clear();
            return this;
        }
    }
}