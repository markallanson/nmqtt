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

namespace NmqttTests.Messages.ConnectAck
{
    /// <summary>
    /// MQTT Message Connect Acknowledgement Tests
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void ConnectionAccepted()
        {
            // our expected result
            var expected = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x0,
            };

            MqttConnectAckMessage msg = new MqttConnectAckMessage().WithReturnCode(MqttConnectReturnCode.ConnectionAccepted);
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // connect ack - compression? always empty
            Assert.Equal<byte>(expected[3], actual[3]); // return code.
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void UnacceptableProtocolVersion()
        {
            // our expected result
            var expected = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x1,
            };

            MqttConnectAckMessage msg = new MqttConnectAckMessage().WithReturnCode(MqttConnectReturnCode.UnacceptedProtocolVersion);
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // connect ack - compression? always empty
            Assert.Equal<byte>(expected[3], actual[3]); // return code.
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void IdentifierRejected()
        {
            // our expected result
            var expected = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x2,
            };

            MqttConnectAckMessage msg = new MqttConnectAckMessage().WithReturnCode(MqttConnectReturnCode.IdentifierRejected);
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // connect ack - compression? always empty
            Assert.Equal<byte>(expected[3], actual[3]); // return code.
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void BrokerUnavailable()
        {
            // our expected result
            var expected = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x3,
            };

            MqttConnectAckMessage msg = new MqttConnectAckMessage().WithReturnCode(MqttConnectReturnCode.BrokerUnavailable);
            Console.WriteLine(msg);

            byte[] actual = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<int>(expected.Length, actual.Length);
            Assert.Equal<byte>(expected[0], actual[0]); // msg type of header
            Assert.Equal<byte>(expected[1], actual[1]); // remaining length
            Assert.Equal<byte>(expected[2], actual[2]); // connect ack - compression? always empty
            Assert.Equal<byte>(expected[3], actual[3]); // return code.
        }
    } 
}
