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
    public class Deserialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void ValidPayload()
        {
            // Message Specs________________
            // <30><0C><00><04>fredhello!
            var sampleMessage = new[]
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

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttPublishMessage>(baseMessage);
            MqttPublishMessage message = (MqttPublishMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<bool>(false, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.Publish, message.Header.MessageType);
            Assert.Equal<int>(12, message.Header.MessageSize);

            // make sure the publish message length matches the expectred size.
            Assert.Equal<int>(6, message.Payload.Message.Count);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void PayloadTooShort()
        {
            // Message Specs________________
            // <30><0C><00><04>fredhello!
            var sampleMessage = new[]
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
            };

            Assert.Throws<InvalidPayloadSizeException>(() => MqttMessage.CreateFrom(sampleMessage));
        }
    } 
}
