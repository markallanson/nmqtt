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

namespace NmqttTests.Messages.PublishRelease
{
    /// <summary>
    /// Tests for the Publish Release message type.
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
            // <40><02><00><04> (Pub Release for Message ID 4)
            var sampleMessage = new[]
            {
                (byte)0x60,
                (byte)0x02,
                (byte)0x0,
                (byte)0x4,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttPublishReleaseMessage>(baseMessage);
            MqttPublishReleaseMessage message = (MqttPublishReleaseMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishRelease, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // make sure the publish message length matches the expectred size.
            Assert.Equal<int>(4, message.VariableHeader.MessageIdentifier);
        }
    } 
}
