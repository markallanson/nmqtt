/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
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
    /// MQTT Message Subscribe Acknowledgement Tests
    /// </summary>
    public class Deserialization
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void SingleQos_AtMostOnce()
        {
            // Message Specs________________
            // <90><03><00><02><00> 
            var sampleMessage = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeAckMessage>(baseMessage);
            MqttSubscribeAckMessage message = (MqttSubscribeAckMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.QosGrants.Count);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Payload.QosGrants[0]);
        }

        [Fact]
        public void SingleQos_AtLeastOnce()
        {
            // Message Specs________________
            // <90><03><00><02><01> 
            var sampleMessage = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x01,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeAckMessage>(baseMessage);
            MqttSubscribeAckMessage message = (MqttSubscribeAckMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.QosGrants.Count);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Payload.QosGrants[0]);
        }

        [Fact]
        public void SingleQos_ExactlyOnce()
        {
            // Message Specs________________
            // <90><03><00><02><02> 
            var sampleMessage = new[]
            {
                (byte)0x90,
                (byte)0x03,
                (byte)0x00,
                (byte)0x02,
                (byte)0x02,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeAckMessage>(baseMessage);
            MqttSubscribeAckMessage message = (MqttSubscribeAckMessage)baseMessage;

            Assert.Equal<int>(1, message.Payload.QosGrants.Count);
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, message.Payload.QosGrants[0]);
        }

        [Fact]
        public void MultipleQos()
        {
            // Message Specs________________
            // <90><03><00><02><00> <01><02>
            var sampleMessage = new[]
            {
                (byte)0x90,
                (byte)0x05,
                (byte)0x00,
                (byte)0x02,
                (byte)0x00,
                (byte)0x01,
                (byte)0x02,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttSubscribeAckMessage>(baseMessage);
            MqttSubscribeAckMessage message = (MqttSubscribeAckMessage)baseMessage;

            Assert.Equal<int>(3, message.Payload.QosGrants.Count);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, message.Payload.QosGrants[0]);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Payload.QosGrants[1]);
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, message.Payload.QosGrants[2]);
        }
    } 
}
