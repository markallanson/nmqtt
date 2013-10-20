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

namespace Nmqtt
{
    /// <summary>
    /// Represents a MQTT message that has been received from a broker.
    /// </summary>
    /// <typeparam name="T">The type of data the payload contains.</typeparam>
    public class MqttReceivedMessage<T>
    {
        private readonly string topic;
        private readonly T      payload;

        /// <summary>
        /// The topic the message was received on.
        /// </summary>
        public string Topic {
            get { return topic; }
        }

        /// <summary>
        /// The payload of the mesage received.
        /// </summary>
        public T Payload {
            get { return payload; }
        }

        /// <summary>
        /// Initializes a new instance of an MqttReceivedMessage class.
        /// </summary>
        /// <param name="topic">The topic the message was received on</param>
        /// <param name="payload">The payload that was received.</param>
        internal MqttReceivedMessage(string topic, T payload) {
            this.topic = topic;
            this.payload = payload;
        }
    }
}