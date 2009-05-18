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

namespace Nmqtt
{
    internal class SubscriptionsManager
    {
        /// <summary>
        /// List of confirmed subscriptions, keyed on the topic name.
        /// </summary>
        private Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();

        /// <summary>
        /// A list of subscriptions that are pending acknowledgement, keyed on the message identifier.
        /// </summary>
        private Dictionary<int, Subscription> pendingSubscriptions = new Dictionary<int, Subscription>();

        /// <summary>
        /// Registers a new subscription with the subscription manager.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="qos"></param>
        /// <returns></returns>
        internal MqttSubscribeMessage RegisterSubscription(string topic, MqttQos qos)
        {
            // check we don't have a pending subscription request for the topic.
            var pendingSubs = from ps in pendingSubscriptions.Values
                              where ps.Topic.Equals(topic)
                              select ps;
            if (pendingSubs.Count<Subscription>() > 0)
            {
                throw new ArgumentException("There is already a pending subscription for this topic");
            }

            // no pending subscription, if we already have a subscription then throw it back out as well
            if (subscriptions.ContainsKey(topic))
            {
                // TODO: we might want to treat this as an ignore/silent confirm because they will be receiving messages for the topic already
                throw new ArgumentException("You are already subscribed for this topic");
            }

            // Add a pending subscription...
            Subscription sub = new Subscription()
            {
                Topic = topic,
                Qos = qos,
                MessageIdentifier = MessageIdentifierDispenser.GetNextMessageIdentifier("subscriptions"),
                CreatedTime = DateTime.Now
            };
            pendingSubscriptions.Add(sub.MessageIdentifier, sub);

            // build a subscribe message for the caller.
            MqttSubscribeMessage msg = new MqttSubscribeMessage()
                .WithMessageIdentifier(sub.MessageIdentifier)
                .ToTopic(sub.Topic)
                .AtQos(sub.Qos);

            return msg;
        }

        /// <summary>
        /// Confirms a subscription has been made with the broker. Marks the sub as confirmed in the subs storage.
        /// </summary>
        /// <param name="subAck"></param>
        internal void ConfirmSubscription(MqttSubscribeAckMessage subAck)
        {
            Subscription sub;
            if (!pendingSubscriptions.TryGetValue(subAck.VariableHeader.MessageIdentifier, out sub))
            {
                throw new ArgumentException(
                    String.Format("There is no pending subscription against message identifier {0}", subAck.VariableHeader.MessageIdentifier));
            }

            subscriptions.Add(sub.Topic, sub);
        }
    }
}
