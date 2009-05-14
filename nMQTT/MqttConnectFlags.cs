/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Represents the connect flags part of the MQTT Variable Header
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification="Keeping terminology consistent with Mqtt Spec")]
    public class MqttConnectFlags
    {
        public bool Reserved1 { get; set; }
        public bool CleanStart { get; set; }
        public bool WillFlag { get; set; }
        public MqttQos WillQos { get; set; }
        public bool WillRetain { get; set; }
        public bool Reserved2 { get; set; }
        public bool Reserved3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectFlags"/> class.
        /// </summary>
        public MqttConnectFlags()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectFlags"/> class configured as per the supplied stream.
        /// </summary>
        /// <param name="connectFlagsStream">The connect flags stream.</param>
        public MqttConnectFlags(Stream connectFlagsStream)
        {
            ReadFrom(connectFlagsStream);
        }

        /// <summary>
        /// Writes the connect flag byte to the supplied stream.
        /// </summary>
        /// <param name="connectFlagsStream">The stream to write to.</param>
        public void WriteTo(Stream connectFlagsStream)
        {
            connectFlagsStream.WriteByte(ConnectFlagByte);
        }

        /// <summary>
        /// Reads the connect flags from the underlying stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        internal void ReadFrom(Stream stream)
        {
            byte connectFlagsByte = (byte)stream.ReadByte();

            Reserved1 = (connectFlagsByte & 1) == 1;
            CleanStart = (connectFlagsByte & 2) == 2;
            WillFlag = (connectFlagsByte & 4) == 4;
            WillQos = (MqttQos)((connectFlagsByte >> 3) & 3);
            WillRetain = (connectFlagsByte & 32) == 32;
            Reserved2 = (connectFlagsByte & 64) == 64;
            Reserved3 = (connectFlagsByte & 128) == 128;       
        }

        /// <summary>
        /// Builds the byte that represents the current connect flags.
        /// </summary>
        private byte ConnectFlagByte
        {
            get
            {
                return (byte)
                    ((Reserved1 ? 1 : 0) |
                    (CleanStart ? 1 : 0) << 1 |
                    (WillFlag ? 1 : 0) << 2 |
                    ((byte)WillQos) << 3 |
                    (WillRetain ? 1 : 0) << 5 |
                    (Reserved2 ? 1 : 0) << 6 |
                    (Reserved3 ? 1 : 0) << 7);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Connect Flags: Reserved1={0}, CleanStart={1}, WillFlag={2}, WillQos={3}, " +
                "WillRetain={4}, Reserved2={5}, Reserved3={6}",
                Reserved1, CleanStart, WillFlag, WillQos, WillRetain, Reserved2, Reserved3);
        }
    }
}
