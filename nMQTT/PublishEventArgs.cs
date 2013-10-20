/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2013 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace Nmqtt {
    /// <summary>
    /// Represents an event argumnts that contains an Mqtt Publish Message.
    /// </summary>
    internal class PublishEventArgs : EventArgs {
        /// <summary>
        /// The message being published.
        /// </summary>
        public MqttPublishMessage PublishMessage { get; set; }

        /// <summary>
        /// Creates a new instance of a PublishEventArgs class.
        /// </summary>
        /// <param name="publishMessage">The MQTT Publish Message that's been published.</param>
        public PublishEventArgs(MqttPublishMessage publishMessage) {
            PublishMessage = publishMessage;
        }
    }
}
