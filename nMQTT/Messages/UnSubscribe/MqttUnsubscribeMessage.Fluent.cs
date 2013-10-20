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
    internal sealed partial class MqttUnsubscribeMessage : MqttMessage
    {
        /// <summary>
        ///     Sets the message identifier on the unsubscribe message.
        /// </summary>
        /// <param name="messageIdentifier">The ID of the message.</param>
        /// <returns>The updated instance of the MqttSubscribeAckMessage.</returns>
        public MqttUnsubscribeMessage WithMessageIdentifier(short messageIdentifier) {
            this.VariableHeader.MessageIdentifier = messageIdentifier;
            return this;
        }

        /// <summary>
        ///     Adds a topic to the list of topics to unsubscribe from.
        /// </summary>
        /// <param name="topic">The topic to unsubscribe.</param>
        /// <returns>An updated instance of the message.</returns>
        public MqttUnsubscribeMessage FromTopic(string topic) {
            this.Payload.AddSubscription(topic);
            return this;
        }

        /// <summary>
        ///     Sets the message up to request acknowledgement from the broker for each topic un-subscription.
        /// </summary>
        /// <returns>An updated instance of the message.</returns>
        public MqttUnsubscribeMessage ExpectAcknowledgement() {
            this.Header.WithQos(MqttQos.AtLeastOnce);
            return this;
        }

        /// <summary>
        ///     Sets the duplicate flag for the message to indicate its a duplicate of a previous message type
        ///     with the same message identifier.
        /// </summary>
        /// <returns>An updated version of the message.</returns>
        public MqttUnsubscribeMessage IsDuplicate() {
            this.Header.IsDuplicate();
            return this;
        }
    }
}