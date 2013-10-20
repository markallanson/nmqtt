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

namespace Nmqtt
{
    internal sealed partial class MqttSubscribeMessage
    {
        private string lastTopic;

        /// <summary>
        ///     Adds a new subscription topic with the AtMostOnce Qos Level. If you want to change the
        ///     Qos level follow this call with a call to AtTopic(MqttQos)
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <returns>An updated instance of the message.</returns>
        public MqttSubscribeMessage ToTopic(string topic) {
            lastTopic = topic;
            this.Payload.AddSubscription(topic, MqttQos.AtMostOnce);
            return this;
        }

        /// <summary>
        ///     Sets the Qos level of the last topic added to the subscription list via a call to ToTopic(string)
        /// </summary>
        /// <param name="qos">The Qos to set the last topic subscription to.</param>
        /// <returns>An update instance of the message.</returns>
        public MqttSubscribeMessage AtQos(MqttQos qos) {
            if (this.Payload.Subscriptions.ContainsKey(lastTopic)) {
                this.Payload.Subscriptions[lastTopic] = qos;
            }
            return this;
        }

        /// <summary>
        ///     Sets the message identifier on the subscribe message.
        /// </summary>
        /// <param name="messageIdentifier">The ID of the message.</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttSubscribeMessage WithMessageIdentifier(short messageIdentifier) {
            this.VariableHeader.MessageIdentifier = messageIdentifier;
            return this;
        }


        /// <summary>
        ///     Sets the message up to request acknowledgement from the broker for each topic subscription.
        /// </summary>
        /// <returns>An updated instance of the message.</returns>
        public MqttSubscribeMessage ExpectAcknowledgement() {
            this.Header.WithQos(MqttQos.AtLeastOnce);
            return this;
        }

        /// <summary>
        ///     Sets the duplicate flag for the message to indicate its a duplicate of a previous message type
        ///     with the same message identifier.
        /// </summary>
        /// <returns>An updated version of the message.</returns>
        public MqttSubscribeMessage IsDuplicate() {
            this.Header.IsDuplicate();
            return this;
        }
    }
}