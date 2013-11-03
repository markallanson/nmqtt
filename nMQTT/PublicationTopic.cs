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

namespace Nmqtt {
    /// <summary>
    ///     Implementation of a Publication topic that performs additional validations
    ///     messages that are published.
    /// </summary>
    internal class PublicationTopic : Topic {
        public PublicationTopic(string topic) 
            : base(topic, ValidateMinLength, ValidateMaxLength, ValidateWildcards) { }

        /// <summary>
        ///     Validates that the topic has no wildcards which are not allowed in publication topics.
        /// </summary>
        /// <param name="topicInstance">The instance to check.</param>
        private static void ValidateWildcards(Topic topicInstance) {
            if (topicInstance.HasWildcards) {
                throw new ArgumentException("Cannot publish to a topic that contains MQTT topic wildcards (# or +)");
            }
        }
    }
}
