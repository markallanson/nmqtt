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
    /// Interface that defines how the punlishing manager publishes messages to the broker and
    /// how it passed on messages that are received from the broker.
    /// </summary>
    internal interface IPublishingManager {
        /// <summary>
        ///     Publish a message to the broker on the specified topic.
        /// </summary>
        /// <param name="topic">The topic to send the message to.</param>
        /// <param name="qualityOfService">The QOS to use when publishing the message.</param>
        /// <param name="data">The message to send.</param>
        /// <returns>The message identifier assigned to the message.</returns>
        short Publish<T, TPayloadConverter>(PublicationTopic topic, MqttQos qualityOfService, T data)
            where TPayloadConverter : IPayloadConverter<T>, new();

        /// <summary>
        /// Event raised when a message has been successfully received and the 
        /// relevant QOS handshake has been completed.
        /// </summary>
        event EventHandler<PublishEventArgs> MessageReceived;
    }
}