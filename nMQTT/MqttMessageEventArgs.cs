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

namespace Nmqtt
{
    /// <summary>
    ///     Describes the event arguments that represent an available MqttMessage.
    /// </summary>
    public class MqttMessageEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets or sets the topic the message was published to.
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        ///     Gets or sets the nessage that was published.
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        ///     Creates a new instance of the MqttMessageEventArgs class.
        /// </summary>
        /// <param name="topic">
        ///     A <see cref="System.String" /> that represents the topic the message was published to.
        /// </param>
        /// <param name="message">
        ///     A <see cref="MqttMessage" /> that represents the message that was published.
        /// </param>
        public MqttMessageEventArgs(string topic, object message) {
            Message = message;
            Topic = topic;
        }
    }
}