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
    /// Implementation of a Publication Topic that performs additional validations
    /// for Publication topics.
    /// </summary>
    internal class PublicationTopic : Topic {
        public PublicationTopic(string topic) : base(topic) {
            if (this.HasWildcards) {
                throw new ArgumentException("Cannot publish to a topic that contains MQTT topic wildcards (# or +)");
            }
        }
    }
}
