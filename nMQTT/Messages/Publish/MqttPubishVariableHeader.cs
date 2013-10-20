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
    ///     Implementation of the variable header for an MQTT Connect message.
    /// </summary>
    internal sealed class MqttPublishVariableHeader : MqttVariableHeader
    {
        /// <summary>
        ///     Stores the standard header
        /// </summary>
        private readonly MqttHeader header;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        public MqttPublishVariableHeader(MqttHeader header) {
            this.header = header;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        /// <param name="header">The messages header.</param>
        /// <param name="variableHeaderStream">A stream containing the header of the message.</param>
        public MqttPublishVariableHeader(MqttHeader header, Stream variableHeaderStream)
            : this(header) {
            ReadFrom(variableHeaderStream);
        }

        /// <summary>
        ///     Returns the read flags for the publish message (topic, messageid)
        /// </summary>
        protected override ReadWriteFlags ReadFlags {
            get {
                if (this.header.Qos == MqttQos.AtLeastOnce || this.header.Qos == MqttQos.ExactlyOnce) {
                    return
                        ReadWriteFlags.TopicName |
                        ReadWriteFlags.MessageIdentifier;
                } else {
                    return ReadWriteFlags.TopicName;
                }
            }
        }

        /// <summary>
        ///     Returns the read flags for the publish message (topic, messageid)
        /// </summary>
        protected override ReadWriteFlags WriteFlags {
            get {
                // Read and write flags are identical for Publish Messages
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
                String.Format("Publish Variable Header: TopicName={0}, MessageIdentifier={1}, VH Length={2}",
                              TopicName, MessageIdentifier, Length);
        }
    }
}