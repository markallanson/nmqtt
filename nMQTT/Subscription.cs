/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
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
    /// <summary>
    /// Entity that captures data related to an individual subscription
    /// </summary>
    public class Subscription
    {
        /// <summary>
        /// The message identifier assigned to the subscription
        /// </summary>
        public short MessageIdentifier { get; set; }

        /// <summary>
        /// The time the subscription was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// The topic that is subscribed to.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The QOS level of the topics subscription
        /// </summary>
        public MqttQos Qos { get; set; }

        /// <summary>
        /// The callback that should be called when messages arrive for the subscription.
        /// </summary>
        public Func<string, object, bool> SubscriptionCallback { get; set; }

        /// <summary>
        /// The class that can process received data and turn it into a specific type data.
        /// </summary>
        public IPublishDataConverter DataProcessor { get; set; }
    }
}
