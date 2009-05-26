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

namespace NmqttTests.Messages.Connect
{
    /// <summary>
    /// MQTT Message Connect Tests
    /// </summary>
    public class Deserialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void BasicDeserialization()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <10><15><00><06>MQIsdp<03><02><00><1E><00><07>andy111
            var sampleMessage = new[]
            {
                (byte)0x10,
                (byte)0x1B,
                (byte)0x0,
                (byte)0x6,
                (byte)'M',
                (byte)'Q',
                (byte)'I',
                (byte)'s',
                (byte)'d',
                (byte)'p',
                (byte)0x3,
                (byte)0x2E,
                (byte)0x0,
                (byte)0x1E,
                (byte)0x0,
                (byte)0x7,
                (byte)'a',
                (byte)'n',
                (byte)'d',
                (byte)'y',
                (byte)'1',
                (byte)'1',
                (byte)'1',
                (byte)0x00,
                (byte)0x01,
                (byte)'m',
                (byte)0x00,
                (byte)0x01,
                (byte)'a'
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttConnectMessage>(baseMessage);
            MqttConnectMessage message = (MqttConnectMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.Connect, message.Header.MessageType);
            Assert.Equal<int>(27, message.Header.MessageSize);

            // validate the variable header
            Assert.Equal<string>("MQIsdp", message.VariableHeader.ProtocolName);
            Assert.Equal<int>(30, message.VariableHeader.KeepAlive);
            Assert.Equal<int>(3, message.VariableHeader.ProtocolVersion);
            Assert.True(message.VariableHeader.ConnectFlags.CleanStart);
            Assert.True(message.VariableHeader.ConnectFlags.WillFlag);
            Assert.True(message.VariableHeader.ConnectFlags.WillRetain);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.VariableHeader.ConnectFlags.WillQos);

            // payload tests
            Assert.Equal<string>("andy111", message.Payload.ClientIdentifier);
            Assert.Equal<string>("m", message.Payload.WillTopic);
            Assert.Equal<string>("a", message.Payload.WillMessage);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void Payload_InvalidClientIdenfierLength()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // <10><15><00><06>MQIsdp<03><02><00><1E><00><07>andy111andy111andy111andy111
            var sampleMessage = new[]
            {
                (byte)0x10,
                (byte)0x15,
                (byte)0x0,
                (byte)0x6,
                (byte)'M',
                (byte)'Q',
                (byte)'I',
                (byte)'s',
                (byte)'d',
                (byte)'p',
                (byte)0x3,
                (byte)0x2,
                (byte)0x0,
                (byte)0x1E,
                (byte)0x0,
                (byte)0x1C,
                (byte)'a',
                (byte)'n',
                (byte)'d',
                (byte)'y',
                (byte)'1',
                (byte)'1',
                (byte)'1',
                (byte)'a',
                (byte)'n',
                (byte)'d',
                (byte)'y',
                (byte)'1',
                (byte)'1',
                (byte)'1',
                (byte)'a',
                (byte)'n',
                (byte)'d',
                (byte)'y',
                (byte)'1',
                (byte)'1',
                (byte)'1',
                (byte)'a',
                (byte)'n',
                (byte)'d',
                (byte)'y',
                (byte)'1',
                (byte)'1',
                (byte)'1' 
            };

            Assert.Throws<ClientIdentifierException>(() => MqttMessage.CreateFrom(sampleMessage));
        }
    } 
}
