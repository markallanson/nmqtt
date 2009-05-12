using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using nMqtt;

namespace nMqttTests
{
    public class MqttMessageTests
    {
        /// <summary>
        /// Tests basic message deserialization from a raw byte array.
        /// </summary>
        [Fact]
        public void MessageDeserialize_Basic()
        {
            // Our test deserialization message, with the following properties. Note this message is not 
            // yet a real MQTT message, because not everything is implemented, but it must be modified
            // and ammeneded as work progresses
            //
            // Message Specs________________
            // Duplicate = true
            // Retain = false
            // Message Type = connect
            // Qos = AtLeastOnce
            // Message Size = 1 byte
            // Message payload = number "10"
            var sampleMessage = new []
            {
                (byte)26,
                (byte)1,
                (byte)10
            };

            MqttMessage message = MqttMessage.Create(sampleMessage);

            // validate the message deserialization
            Assert.Equal<bool>(true, message.Header.Duplicate);
            Assert.Equal<bool>(false, message.Header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, message.Header.Qos);
            Assert.Equal<MqttMessageType>(MqttMessageType.Connect, message.Header.MessageType);
            Assert.Equal<int>(1, message.Header.PayloadSize);
        }
    }
}
