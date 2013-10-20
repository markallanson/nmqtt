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
using System.IO;

namespace Nmqtt
{
    /// <summary>
    ///     Implementation of the variable header for an MQTT SubscribeAck message.
    /// </summary>
    internal sealed class MqttSubscribeAckVariableHeader : MqttVariableHeader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttSubscribeAckVariableHeader" /> class.
        /// </summary>
        public MqttSubscribeAckVariableHeader() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttSubscribeAckVariableHeader" /> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttSubscribeAckVariableHeader(Stream headerStream)
            : base(headerStream) {}

        /// <summary>
        ///     Returns the read flags for the SubscribeAck variabe header (prot name, version, SubscribeAck, keepalive)
        /// </summary>
        protected override MqttVariableHeader.ReadWriteFlags ReadFlags {
            get { return ReadWriteFlags.MessageIdentifier; }
        }

        /// <summary>
        ///     Returns the write flags for the SubscribeAck variabe header (prot name, version, SubscribeAck, keepalive)
        /// </summary>
        protected override MqttVariableHeader.ReadWriteFlags WriteFlags {
            get {
                // we read and write the same values on the SubscribeAck header.
                return ReadFlags;
            }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString() {
            return
                String.Format("SubscribeAck Variable Header: Message Identifier={0}", MessageIdentifier);
        }
    }
}