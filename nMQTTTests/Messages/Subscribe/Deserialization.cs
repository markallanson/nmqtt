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

namespace NmqttTests.Messages.Subscribe
{
    /// <summary>
    /// MQTT Message Subscribe Tests
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
            // <82><09><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x09,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x00,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Payload.Subscriptions["fred"]);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MultiTopic()
        {
            // Message Specs________________
            // <82><10><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x10,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x00,
                (byte)0x00,
                (byte)0x04,
                (byte)'m',
                (byte)'a',
                (byte)'r',
                (byte)'k',
                (byte)0x00,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(2, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Payload.Subscriptions["fred"]);
            Assert.True(message.Payload.Subscriptions.ContainsKey("mark"));
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Payload.Subscriptions["mark"]);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleTopic_AtLeastOnceQos()
        {
            // Message Specs________________
            // <82><09><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x09,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x01,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Payload.Subscriptions["fred"]);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MultiTopic_AtLeastOnceQos()
        {
            // Message Specs________________
            // <82><10><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x10,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x01,
                (byte)0x00,
                (byte)0x04,
                (byte)'m',
                (byte)'a',
                (byte)'r',
                (byte)'k',
                (byte)0x01,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(2, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Payload.Subscriptions["fred"]);
            Assert.True(message.Payload.Subscriptions.ContainsKey("mark"));
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Payload.Subscriptions["mark"]);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleTopic_ExactlyOnce()
        {
            // Message Specs________________
            // <82><09><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x09,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x02,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, message.Payload.Subscriptions["fred"]);
        }

        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MultiTopic_ExactlyOnce()
        {
            // Message Specs________________
            // <82><10><00><02><00><04>fred<00> (subscribe to topic fred at qos 0)
            var sampleMessage = new[]
            {
                (byte)0x82,
                (byte)0x10,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'f',
                (byte)'r',
                (byte)'e',
                (byte)'d',
                (byte)0x02,
                (byte)0x00,
                (byte)0x04,
                (byte)'m',
                (byte)'a',
                (byte)'r',
                (byte)'k',
                (byte)0x02,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeMessage>(baseMessage);
            MqttSubscribeMessage message = (MqttSubscribeMessage)baseMessage;

            Assert.Equal<int>(2, message.Payload.Subscriptions.Count);
            Assert.True(message.Payload.Subscriptions.ContainsKey("fred"));
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, message.Payload.Subscriptions["fred"]);
            Assert.True(message.Payload.Subscriptions.ContainsKey("mark"));
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, message.Payload.Subscriptions["mark"]);
        }

    } 
}
