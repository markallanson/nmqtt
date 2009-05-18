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
using Xunit;
using Nmqtt;

namespace NmqttTests.Messages.Publish
{
    /// <summary>
    /// MQTT Message Publish Tests
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void ValidPayload()
        {
            // expected response
            var expected = new[]
            {
                (byte)0x30,
                (byte)0x0C,
                (byte)0x0,
                (byte)0x4,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                // message payload is here
                (byte)'h',
                (byte)'e',
                (byte)'l',
                (byte)'l',
                (byte)'o',
                (byte)'!',
            };

            MqttMessage msg = new MqttPublishMessage()
                .ToTopic("fred")
                .PublishData(new[] 
                    {                 
                        (byte)'h',
                        (byte)'e',
                        (byte)'l',
                        (byte)'l',
                        (byte)'o',
                        (byte)'!',
                    }
                );
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header + other bits
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // first topic length byte
            Assert.Equal<byte>(expected[3], actual[3]); // second topic length byte
            Assert.Equal<byte>(expected[4], actual[4]); // f
            Assert.Equal<byte>(expected[5], actual[5]); // r
            Assert.Equal<byte>(expected[6], actual[6]); // e
            Assert.Equal<byte>(expected[7], actual[7]); // d
            Assert.Equal<byte>(expected[8], actual[8]); // h
            Assert.Equal<byte>(expected[9], actual[9]); // e
            Assert.Equal<byte>(expected[10], actual[10]); // l
            Assert.Equal<byte>(expected[11], actual[11]); // l
            Assert.Equal<byte>(expected[12], actual[12]); // o
            Assert.Equal<byte>(expected[13], actual[13]); // !
        }
    } 
}
