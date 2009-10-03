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

namespace NmqttTests.Messages.Unsubscribe
{
    /// <summary>
    /// MQTT Message Unsubscribe Tests
    /// </summary>
    public class Deserialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleTopic()
        {
            // Message Specs________________
            // <A2><08><00><03><00><04>fred (Unsubscribe to topic fred)
            var sampleMessage = new[]
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

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttUnsubscribeMessage>(baseMessage);
            MqttUnsubscribeMessage message = (MqttUnsubscribeMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.Contains("fred"));
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MultiTopic()
        {
            // Message Specs________________
            // <A2><0E><00><03><00><04>fred<00><04>mark (Unsubscribe to topic fred, mark)
            var sampleMessage = new[]
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

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttUnsubscribeMessage>(baseMessage);
            MqttUnsubscribeMessage message = (MqttUnsubscribeMessage)baseMessage;

            Assert.Equal<int>(2, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.Contains("fred"));
            Assert.True(message.Payload.Subscriptions.Contains("mark"));
        }
    } 
}
