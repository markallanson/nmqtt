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
using NmqttTests;

namespace nMqttTests.Encoding
{
    public class MqttEncoding
    {
        /// <summary>
        /// Tests the MqttEncoding GetBytes method
        /// </summary>
        [Fact]
        public void GetBytes()
        {
            System.Text.Encoding enc = GetEncoding();
            var bytes = enc.GetBytes("abc");

            Assert.Equal<int>(5, bytes.Length);
            Assert.Equal<byte>(0, bytes[0]);
            Assert.Equal<byte>(3, bytes[1]);
            Assert.Equal<byte>((byte)'a', bytes[2]);
            Assert.Equal<byte>((byte)'b', bytes[3]);
            Assert.Equal<byte>((byte)'c', bytes[4]);
        }

        /// <summary>
        /// Tests the MqttEncoding GetBytes method
        /// </summary>
        [Fact]
        public void GetByteCount()
        {
            System.Text.Encoding enc = GetEncoding();
            var byteCount = enc.GetByteCount("abc");

            Assert.Equal<int>(5, byteCount);
        }

        /// <summary>
        /// Tests the MqttEncoding GetString method
        /// </summary>
        [Fact]
        public void GetString()
        {
            var strBytes = new[] 
            {
                (byte)'a',
                (byte)'b',
                (byte)'c'
            };

            // TODO: Decide whether the input to GetString should include the length bytes...
            System.Text.Encoding enc = GetEncoding();
            var message = enc.GetString(strBytes);

            Assert.Equal<string>("abc", message);
        }

        /// <summary>
        /// Tests the MqttEncoding GetCharCount method with a valid input
        /// </summary>
        [Fact]
        public void GetCharCount_ValidLength()
        {
            var strBytes = new[] 
            {
                (byte)0,
                (byte)3,
                (byte)'a',
                (byte)'b',
                (byte)'c'
            };

            System.Text.Encoding enc = GetEncoding();
            var count = enc.GetCharCount(strBytes);

            Assert.Equal<int>(3, count);
        }

        /// <summary>
        /// Tests the MqttEncoding GetCharCount method with a byte array that is too short.
        /// </summary>
        [Fact]
        public void GetCharCount_InvalidLength()
        {
            var strBytes = new[] 
            {
                (byte)0
            };

            System.Text.Encoding enc = GetEncoding();
            Assert.Throws<ArgumentException>(() => enc.GetCharCount(strBytes));
        }

        /// <summary>
        /// Tests that characters outside the allowed range in an Mqtt string are rejected.
        /// </summary>
        [Fact]
        public void ExtendedCharactersInitiateFailure()
        {
            var extStr = "©";
            System.Text.Encoding enc = GetEncoding();

            Assert.Throws<ArgumentException>(
                () => {
                    enc.GetBytes(extStr);
                });
        }

        private System.Text.Encoding GetEncoding()
        {
            Type encType = Type.GetType("Nmqtt.Encoding.MqttEncoding, Nmqtt");
            return (System.Text.Encoding)Activator.CreateInstance(encType);
        }
    }
}
