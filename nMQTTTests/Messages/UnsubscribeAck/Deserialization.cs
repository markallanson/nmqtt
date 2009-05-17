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

namespace NmqttTests.Messages.UnsubscribeAck
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
        public void ValidPayload()
        {
            // Message Specs________________
            // <B0><02><00><04> (Subscribe ack for message id 4)
            var sampleMessage = new[]
            {
                (byte)0xB0,
                (byte)0x02,
                (byte)0x0,
                (byte)0x4,
            };

            MqttMessage baseMessage = MqttMessage.CreateFrom(sampleMessage);

            Console.WriteLine(baseMessage.ToString());

            // check that the message was correctly identified as a connect message.
            Assert.IsType<MqttUnsubscribeAckMessage>(baseMessage);
            MqttUnsubscribeAckMessage message = (MqttUnsubscribeAckMessage)baseMessage;

            // validate the message deserialization
            Assert.Equal<MqttMessageType>(MqttMessageType.UnsubscribeAck, message.Header.MessageType);
            Assert.Equal<int>(2, message.Header.MessageSize);

            // make sure the UnSubscribe message length matches the expectred size.
            Assert.Equal<int>(4, message.VariableHeader.MessageIdentifier);
        }
    } 
}
