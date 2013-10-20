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
    /// <summary>
    ///     Implementation of an MQTT Publish Complete Message.
    /// </summary>
    internal sealed partial class MqttPublishCompleteMessage
    {
        /// <summary>
        ///     Sets the message identifier on the publishComplete message.
        /// </summary>
        /// <param name="messageIdentifier">The ID of the message.</param>
        /// <returns>The updated instance of the MqttPublishCompleteMessage.</returns>
        public MqttPublishCompleteMessage WithMessageIdentifier(short messageIdentifier) {
            this.VariableHeader.MessageIdentifier = messageIdentifier;
            return this;
        }
    }
}