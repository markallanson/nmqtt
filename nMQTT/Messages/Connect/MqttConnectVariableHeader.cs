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
    internal sealed class MqttConnectVariableHeader : MqttVariableHeader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        public MqttConnectVariableHeader() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttConnectVariableHeader(Stream headerStream)
            : base(headerStream) {}

        /// <summary>
        ///     Returns the read flags for the connect variabe header (prot name, version, connect, keepalive)
        /// </summary>
        protected override MqttVariableHeader.ReadWriteFlags ReadFlags {
            get {
                return
                    ReadWriteFlags.ProtocolName |
                    ReadWriteFlags.ProtocolVersion |
                    ReadWriteFlags.ConnectFlags |
                    ReadWriteFlags.KeepAlive;
            }
        }

        /// <summary>
        ///     Returns the write flags for the connect variabe header (prot name, version, connect, keepalive)
        /// </summary>
        protected override MqttVariableHeader.ReadWriteFlags WriteFlags {
            get {
                // we read and write the same values on the connect header.
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
                String.Format(
                    "Connect Variable Header: ProtocolName={0}, ProtocolVersion={1}, ConnectFlags=({2}), KeepAlive={3}",
                    ProtocolName, ProtocolVersion, ConnectFlags, KeepAlive);
        }
    }
}