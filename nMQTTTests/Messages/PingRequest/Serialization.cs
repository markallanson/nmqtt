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
using NmqttTests.Messages;

namespace NmqttTests.Messages.PingRequest
{
    /// <summary>
    /// MQTT Message Ping Request Tests
    /// </summary>
    public class Serialization
    {

        [Fact]
        public void BasicSerialization()
        {
            MqttPingRequestMessage pingReqMsg = new MqttPingRequestMessage();
            byte[] bytes = MessageSerializationHelper.GetMessageBytes(pingReqMsg);

            Assert.Equal<byte>(192, bytes[0]);
        }

    } 
}
