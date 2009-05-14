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

namespace NmqttTests
{
    /// <summary>
    /// MQTT Message Tests with sample input data provided by andy@stanford-clark.com
    /// </summary>
    public class MqttMessage_UnimplementedMessageTypeTests
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void Deserialize_Message_MessageType_UnsubscribeAck_ValidPayload()
        {
            // Message Specs________________
            // <B0><02><00><04> (Subscribe ack for message id 4)
            var sampleMessage = new[]
            {
                (byte)0xFF,
                (byte)0x02,
                (byte)0x0,
                (byte)0x4,
            };

            Assert.Throws<InvalidHeaderException>(() => MqttMessage.CreateFrom(sampleMessage));
        }
    } 
}
