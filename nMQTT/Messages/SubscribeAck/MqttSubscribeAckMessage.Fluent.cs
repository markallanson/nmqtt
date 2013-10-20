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
    internal sealed partial class MqttSubscribeAckMessage
    {
        /// <summary>
        ///     Sets the message identifier on the subscribe acknowledgement message.
        /// </summary>
        /// <param name="messageIdentifier">The ID of the message.</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttSubscribeAckMessage WithMessageIdentifier(short messageIdentifier) {
            this.VariableHeader.MessageIdentifier = messageIdentifier;
            return this;
        }

        /// <summary>
        ///     Adds a subscription grant to the message.
        /// </summary>
        /// <param name="qosGranted">The granted Qos to add.</param>
        /// <returns>The updated instance of the message.</returns>
        public MqttSubscribeAckMessage AddQosGrant(MqttQos qosGranted) {
            this.Payload.AddGrant(qosGranted);
            return this;
        }
    }
}