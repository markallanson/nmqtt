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
    ///     Represents the connect flags part of the MQTT Variable Header
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags"
        , Justification = "Keeping terminology consistent with Mqtt Spec")]
    internal class MqttConnectFlags
    {
        public bool Reserved1 { get; set; }
        public bool CleanStart { get; set; }
        public bool WillFlag { get; set; }
        public MqttQos WillQos { get; set; }
        public bool WillRetain { get; set; }
        public bool PasswordFlag { get; set; }
        public bool UsernameFlag { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectFlags" /> class.
        /// </summary>
        public MqttConnectFlags() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectFlags" /> class configured as per the supplied stream.
        /// </summary>
        /// <param name="connectFlagsStream">The connect flags stream.</param>
        public MqttConnectFlags(Stream connectFlagsStream) {
            ReadFrom(connectFlagsStream);
        }

        /// <summary>
        ///     Writes the connect flag byte to the supplied stream.
        /// </summary>
        /// <param name="connectFlagsStream">The stream to write to.</param>
        public void WriteTo(Stream connectFlagsStream) {
            connectFlagsStream.WriteByte(ConnectFlagByte);
        }

        /// <summary>
        ///     Reads the connect flags from the underlying stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        internal void ReadFrom(Stream stream) {
            var connectFlagsByte = (byte) stream.ReadByte();

            Reserved1 = (connectFlagsByte & 1) == 1;
            CleanStart = (connectFlagsByte & 2) == 2;
            WillFlag = (connectFlagsByte & 4) == 4;
            WillQos = (MqttQos) ((connectFlagsByte >> 3) & 3);
            WillRetain = (connectFlagsByte & 32) == 32;
            PasswordFlag = (connectFlagsByte & 64) == 64;
            UsernameFlag = (connectFlagsByte & 128) == 128;       
        }

        /// <summary>
        ///     Builds the byte that represents the current connect flags.
        /// </summary>
        private byte ConnectFlagByte {
            get {
                return (byte)
                    ((Reserved1 ? 1 : 0) |
                    (CleanStart ? 1 : 0) << 1 |
                    (WillFlag ? 1 : 0) << 2 |
                    ((byte)WillQos) << 3 |
                    (WillRetain ? 1 : 0) << 5 |
                    (PasswordFlag ? 1 : 0) << 6 |
                    (UsernameFlag ? 1 : 0) << 7);
            }
        }

        /// <summary>
        ///     Gets the length of data written when WriteTo is called.
        /// </summary>
        /// <returns></returns>
        internal static int GetWriteLength() {
            return 1;
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString() {
            return String.Format("Connect Flags: Reserved1={0}, CleanStart={1}, WillFlag={2}, WillQos={3}, " +
                                 "WillRetain={4}, PasswordFlag={5}, UserNameFlag={6}",
                                  Reserved1, CleanStart, WillFlag, WillQos, WillRetain, PasswordFlag, UsernameFlag);
        }
    }
}
