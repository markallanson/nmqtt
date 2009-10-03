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

namespace NmqttTests.Messages.SubscribeAck
{
    /// <summary>
    /// MQTT Message Subscribe Acknowledgement serialization Tests
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Tests basic message serialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleQos_AtMostOnce()
        {
            var expected = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
            };

            MqttMessage msg = new MqttSubscribeAckMessage()
                .WithMessageIdentifier(2)
                .AddQosGrant(MqttQos.AtMostOnce);

            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // header byte 1
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // message id b1
            Assert.Equal<byte>(expected[3], actual[3]); // message id b2
            Assert.Equal<byte>(expected[4], actual[4]); // QOS
        }

        [Fact]
        public void SingleQos_AtLeastOnce()
        {
            var expected = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x01,
            };

            MqttMessage msg = new MqttSubscribeAckMessage()
                .WithMessageIdentifier(2)
                .AddQosGrant(MqttQos.AtLeastOnce);

            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // header byte 1
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // message id b1
            Assert.Equal<byte>(expected[3], actual[3]); // message id b2
            Assert.Equal<byte>(expected[4], actual[4]); // QOS
        }

        [Fact]
        public void SingleQos_ExactlyOnce()
        {
            var expected = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x02,
            };

            MqttMessage msg = new MqttSubscribeAckMessage()
                .WithMessageIdentifier(2)
                .AddQosGrant(MqttQos.ExactlyOnce);

            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // header byte 1
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // message id b1
            Assert.Equal<byte>(expected[3], actual[3]); // message id b2
            Assert.Equal<byte>(expected[4], actual[4]); // QOS
        }

        [Fact]
        public void MultipleQos()
        {
            var expected = new[]
            {
                (byte)0x90,
                (byte)0x05,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x01,
                (byte)0x02,
            };

            MqttMessage msg = new MqttSubscribeAckMessage()
                .WithMessageIdentifier(2)
                .AddQosGrant(MqttQos.AtMostOnce)
                .AddQosGrant(MqttQos.AtLeastOnce)
                .AddQosGrant(MqttQos.ExactlyOnce);

            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // header byte 1
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // message id b1
            Assert.Equal<byte>(expected[3], actual[3]); // message id b2
            Assert.Equal<byte>(expected[4], actual[4]); // QOS 1 (Most)
            Assert.Equal<byte>(expected[5], actual[5]); // QOS 1 (Least)
            Assert.Equal<byte>(expected[6], actual[6]); // QOS 1 (Exactly)
        }

        [Fact]
        public void ClearGrantsClearsGrants()
        {
            MqttSubscribeAckMessage msg = new MqttSubscribeAckMessage()
                .WithMessageIdentifier(2)
                .AddQosGrant(MqttQos.AtMostOnce)
                .AddQosGrant(MqttQos.AtLeastOnce)
                .AddQosGrant(MqttQos.ExactlyOnce);

            Assert.Equal<int>(3, msg.Payload.QosGrants.Count);
            msg.Payload.ClearGrants();
            Assert.Equal<int>(0, msg.Payload.QosGrants.Count);
        }

    } 
}
