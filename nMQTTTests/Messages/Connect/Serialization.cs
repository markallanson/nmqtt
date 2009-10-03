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

namespace NmqttTests.Messages.Connect
{
    /// <summary>
    /// MQTT Message Connect Tests
    /// </summary>
    public class Serialization
    {

        [Fact]
        public void BasicSerialization()
        {
            MqttConnectMessage msg = new MqttConnectMessage()
                .WithClientIdentifier("mark")
                .KeepAliveFor(30)
                .StartClean();

            Console.WriteLine(msg);

            byte[] mb = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<byte>(0x10, mb[0]);
            // VH will = 12, Msg = 6
            Assert.Equal<byte>(18, mb[1]);
        }

        [Fact]
        public void WithWillSet()
        {
            MqttConnectMessage msg = new MqttConnectMessage()
                .WithProtocolName("MQIsdp")
                .WithProtocolVersion(3)
                .WithClientIdentifier("mark")
                .KeepAliveFor(30)
                .StartClean()
                .Will()
                .WithWillQos(MqttQos.AtLeastOnce)
                .WithWillRetain()
                .WithWillTopic("willTopic")
                .WithWillMessage("willMessage");

            Console.WriteLine(msg);

            byte[] mb = MessageSerializationHelper.GetMessageBytes(msg);

            Assert.Equal<byte>(0x10, mb[0]);
            // VH will = 12, Msg = 6
            Assert.Equal<byte>(42, mb[1]);

        }
    } 
}
