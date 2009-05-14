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
