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

using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    /// Implementation of an MQTT Publish Message, used for publishing telemetry data along a live MQTT stream.
    /// </summary>
    public sealed partial class MqttPublishMessage : MqttMessage
    {
        /// <summary>
        /// Sets the topic to publish data to.
        /// </summary>
        /// <param name="topicName">The name of the topic to publish.</param>
        /// <returns>The updated instance of the message with the topic name set.</returns>
        public MqttPublishMessage ToTopic(string topicName)
        {
            this.VariableHeader.TopicName = topicName;
            return this;
        }

        /// <summary>
        /// Appends data to publish to the end of the current message payload.
        /// </summary>
        /// <param name="publishData">The data to append to the end of the published data</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttPublishMessage PublishData(byte[] data)
        {
            this.Payload.Message.AddRange(new System.Collections.ObjectModel.Collection<byte>(data));
            return this;
        }

        /// <summary>
        /// Removes the current published data.
        /// </summary>
        /// <returns>The updated instance of the message with published data cleared</returns>
        public MqttPublishMessage ClearPublishData()
        {
            this.Payload.Message.Clear();
            return this;
        }
    }
}
