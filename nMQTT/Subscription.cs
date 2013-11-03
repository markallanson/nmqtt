/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2013 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/
using System;

namespace Nmqtt
{
    /// <summary>
    ///     Entity that captures data related to an individual subscription
    /// </summary>
    internal class Subscription
    {
        /// <summary>
        ///     The message identifier assigned to the subscription
        /// </summary>
        public short                                    MessageIdentifier { get; set; }

        /// <summary>
        ///     The time the subscription was created.
        /// </summary>
        public DateTime                                 CreatedTime { get; set; }

        /// <summary>
        ///     The Topic that is subscribed to.
        /// </summary>
        public SubscriptionTopic                        Topic { get; set; }

        /// <summary>
        ///     The QOS level of the topics subscription
        /// </summary>
        public MqttQos                                  Qos { get; set; }

        /// <summary>
        /// The observable that receives messages from the broker.
        /// </summary>
        public IObservable<MqttReceivedMessage<byte[]>> Observable { get; set; }
    }
}