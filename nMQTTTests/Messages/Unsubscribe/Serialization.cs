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

namespace NmqttTests.Messages.Unsubscribe
{
    /// <summary>
    /// MQTT Message Unsubscribe Tests
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleTopic()
        {
            // simple single topic unsubscribe message
            var expected = new[]
            {
                (byte)0xA2,
                (byte)0x08,
                (byte)0x00,
                (byte)0x03,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
            };

            MqttMessage msg = new MqttUnsubscribeMessage()
                .FromTopic("fred")
                .WithMessageIdentifier(3)
                .ExpectAcknowledgement();
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // Start of VH: MsgID Byte1
            Assert.Equal<byte>(expected[3], actual[3]); // MsgID Byte 2
            Assert.Equal<byte>(expected[4], actual[4]); // Topic Length B1
            Assert.Equal<byte>(expected[5], actual[5]); // Topic Length B2
            Assert.Equal<byte>(expected[6], actual[6]); // f
            Assert.Equal<byte>(expected[7], actual[7]); // r
            Assert.Equal<byte>(expected[8], actual[8]); // e
            Assert.Equal<byte>(expected[9], actual[9]); // d
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MultiTopic()
        {
            // double topic unsubscribe
            var expected = new[]
            {
                (byte)0xA2,
                (byte)0x0E,
                (byte)0x00,
                (byte)0x03,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x00,
                (byte)0x04,
                (byte)'m',
                (byte)'a',
                (byte)'r',
                (byte)'k',
            };

            MqttMessage msg = new MqttUnsubscribeMessage()
                .FromTopic("fred")
                .FromTopic("mark")
                .WithMessageIdentifier(3)
                .ExpectAcknowledgement();
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // Start of VH: MsgID Byte1
            Assert.Equal<byte>(expected[3], actual[3]); // MsgID Byte 2
            Assert.Equal<byte>(expected[4], actual[4]); // Topic Length B1
            Assert.Equal<byte>(expected[5], actual[5]); // Topic Length B2
            Assert.Equal<byte>(expected[6], actual[6]); // f
            Assert.Equal<byte>(expected[7], actual[7]); // r
            Assert.Equal<byte>(expected[8], actual[8]); // e
            Assert.Equal<byte>(expected[9], actual[9]); // d
            Assert.Equal<byte>(expected[10], actual[10]); // Topic Length B1
            Assert.Equal<byte>(expected[11], actual[11]); // Topic Length B2
            Assert.Equal<byte>(expected[12], actual[12]); // f
            Assert.Equal<byte>(expected[13], actual[13]); // r
            Assert.Equal<byte>(expected[14], actual[14]); // e
            Assert.Equal<byte>(expected[15], actual[15]); // d
        }

        [Fact]
        public void ClearSubscriptions()
        {
            MqttUnsubscribeMessage msg = new MqttUnsubscribeMessage()
                .FromTopic("fred")
                .FromTopic("mark")
                .WithMessageIdentifier(3)
                .ExpectAcknowledgement();
            Console.WriteLine(msg);

            Assert.Equal<int>(2, msg.Payload.Subscriptions.Count);

            msg.Payload.ClearSubscriptions();

            Assert.Equal<int>(0, msg.Payload.Subscriptions.Count);
        }
    } 
}
