/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
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
using Xunit;
using Nmqtt;
using System.IO;

namespace NmqttTests.MessageComponents.VariableHeader
{
    public class MqttConnectFlagTests
    {

        [Fact]
        public void MqttConnectFlags_WillQos_AtMostOnce()
        {
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, GetConnectFlags(0).WillQos);
        }

        [Fact]
        public void MqttConnectFlags_WillQos_AtLeastOnce()
        {
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, GetConnectFlags(8).WillQos);
        }

        [Fact]
        public void MqttConnectFlags_WillQos_ExactlyOnce()
        {
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, GetConnectFlags(16).WillQos);
        }

        [Fact]
        public void MqttConnectFlags_WillQos_Reserved1()
        {
            Assert.Equal<MqttQos>(MqttQos.Reserved1, GetConnectFlags(24).WillQos);
        }

        [Fact]
        public void MqttConnectFlags_Reserved1_true()
        {
            Assert.True(GetConnectFlags(1).Reserved1);
        }

        [Fact]
        public void MqttConnectFlags_Reserved1_false()
        {
            Assert.False(GetConnectFlags(0).Reserved1);
        }

        [Fact]
        public void MqttConnectFlags_PasswordFlag_true()
        {
            Assert.True(GetConnectFlags(64).PasswordFlag);
        }

        [Fact]
        public void MqttConnectFlags_PasswordFlag_false()
        {
            Assert.False(GetConnectFlags(0).PasswordFlag);
        }

        [Fact]
        public void MqttConnectFlags_UsernameFlag_true()
        {
            Assert.True(GetConnectFlags(128).UsernameFlag);
        }

        [Fact]
        public void MqttConnectFlags_UsernameFlag_false()
        {
            Assert.False(GetConnectFlags(0).UsernameFlag);
        }

        [Fact]
        public void MqttConnectFlags_CleanStart_true()
        {
            Assert.True(GetConnectFlags(2).CleanStart);
        }

        [Fact]
        public void MqttConnectFlags_CleanStart_false()
        {
            Assert.False(GetConnectFlags(1).CleanStart);
        }

        [Fact]
        public void MqttConnectFlags_WillRetain_true()
        {
            Assert.True(GetConnectFlags(32).WillRetain);
        }

        [Fact]
        public void MqttConnectFlags_WillRetain_false()
        {
            Assert.False(GetConnectFlags(1).WillRetain);
        }

        [Fact]
        public void MqttConnectFlags_WillFlag_true()
        {
            Assert.True(GetConnectFlags(4).WillFlag);
        }

        [Fact]
        public void MqttConnectFlags_WillFlag_false()
        {
            Assert.False(GetConnectFlags(1).WillFlag);
        }

        /// <summary>
        /// Gets the connect flags for a specific byte value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private MqttConnectFlags GetConnectFlags(byte value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte(value);
                stream.Seek(0, SeekOrigin.Begin);
                return new MqttConnectFlags(stream);
            }
        }
    }
}
