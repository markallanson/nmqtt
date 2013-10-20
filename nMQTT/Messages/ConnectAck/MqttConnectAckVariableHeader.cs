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
    ///     Implementation of the variable header for an MQTT ConnectAck message.
    /// </summary>
    internal sealed class MqttConnectAckVariableHeader : MqttVariableHeader
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        public MqttConnectAckVariableHeader() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectVariableHeader" /> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttConnectAckVariableHeader(Stream headerStream)
            : base(headerStream) {}

        /// <summary>
        ///     Writes the variable header for an MQTT Connect message to the supplied stream.
        /// </summary>
        /// <param name="variableHeaderStream"></param>
        public override void WriteTo(System.IO.Stream variableHeaderStream) {
            // unused additional "compression" byte used within the variable header acknowledgement.
            variableHeaderStream.WriteByte(0);
            WriteReturnCode(variableHeaderStream);
        }

        /// <summary>
        ///     Creates a variable header from the specified header stream.
        /// </summary>
        /// <param name="variableHeaderStream">The header stream.</param>
        public override void ReadFrom(System.IO.Stream variableHeaderStream) {
            // unused additional "compression" byte used within the variable header acknowledgement.
            variableHeaderStream.ReadByte();
            ReadReturnCode(variableHeaderStream);
        }

        /// <summary>
        ///     Gets the length of the write data when WriteTo will be called.
        /// </summary>
        /// <returns>
        ///     The length of data witten by the call to GetWriteLength
        /// </returns>
        /// <remarks>
        ///     This method is overriden by the ConnectAckVariableHeader because the variable header of this
        ///     message type, for some reason, contains an extra byte that is not present in the variable
        ///     header spec, meaning we have to do some custom serialization and deserialization.
        /// </remarks>
        public override int GetWriteLength() {
            return sizeof (MqttConnectReturnCode) + 1; // +1 for compression level byte
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString() {
            return
                String.Format("Connect Variable Header: TopicNameCompressionResponse={0}, ReturnCode={1}",
                              0, ReturnCode);
        }
    }
}