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
    public class Deserialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void ConnectionAccepted()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <20><02><00><00>
            var sampleMessage = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x0,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttConnectAckMessage>(baseMessage);
            MqttConnectAckMessage message = (MqttConnectAckMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // validate the variable header
            //Assert.Equal<int>(30, message.VariableHeader.KeepAlive);
            Assert.Equal<MqttConnectReturnCode>(MqttConnectReturnCode.ConnectionAccepted, message.VariableHeader.ReturnCode);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void UnacceptableProtocolVersion()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <20><02><00><00>
            var sampleMessage = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x1,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttConnectAckMessage>(baseMessage);
            MqttConnectAckMessage message = (MqttConnectAckMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // validate the variable header
            //Assert.Equal<int>(30, message.VariableHeader.KeepAlive);
            Assert.Equal<MqttConnectReturnCode>(MqttConnectReturnCode.UnacceptedProtocolVersion, message.VariableHeader.ReturnCode);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void IdentifierRejected()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <20><02><00><00>
            var sampleMessage = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x2,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttConnectAckMessage>(baseMessage);
            MqttConnectAckMessage message = (MqttConnectAckMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // validate the variable header
            //Assert.Equal<int>(30, message.VariableHeader.KeepAlive);
            Assert.Equal<MqttConnectReturnCode>(MqttConnectReturnCode.IdentifierRejected, message.VariableHeader.ReturnCode);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void BrokerUnavailable()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <20><02><00><00>
            var sampleMessage = new[]
            {
                (byte)0x20,
                (byte)0x02,
                (byte)0x0,
                (byte)0x3,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttConnectAckMessage>(baseMessage);
            MqttConnectAckMessage message = (MqttConnectAckMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // validate the variable header
            //Assert.Equal<int>(30, message.VariableHeader.KeepAlive);
            Assert.Equal<MqttConnectReturnCode>(MqttConnectReturnCode.BrokerUnavailable, message.VariableHeader.ReturnCode);
        }
    } 
}
