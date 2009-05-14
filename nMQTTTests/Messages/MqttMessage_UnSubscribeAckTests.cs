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
    public class MqttMessage_UnsubscribeAckTests
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
