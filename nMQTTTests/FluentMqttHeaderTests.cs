using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Nmqtt;

namespace NmqttTests
{
    public class FluentMqttHeaderTests
    {
        [Fact]
        public void FluentMqttHeader_SettingDuplicate()
        {
            MqttHeader header = new MqttHeader().IsDuplicate();
            Assert.True(header.Duplicate);
        }

        [Fact]
        public void FluentMqttHeader_SettingQos()
        {
            MqttHeader header = new MqttHeader().WithQos(MqttQos.AtMostOnce);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
        }

        [Fact]
        public void FluentMqttHeader_SettingMessageType()
        {
            MqttHeader header = new MqttHeader().AsType(MqttMessageType.PublishComplete);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
        }

        [Fact]
        public void FluentMqttHeader_SettingRetain()
        {
            MqttHeader header = new MqttHeader().ShouldBeRetained();
            Assert.True(header.Retain);
        }

        [Fact]
        public void FluentMqttHeader_UseAll()
        {
            MqttHeader header
                = new MqttHeader()
                    .AsType(MqttMessageType.PublishComplete)
                    .WithQos(MqttQos.AtMostOnce)
                    .IsDuplicate()
                    .ShouldBeRetained();

            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
            Assert.True(header.Retain);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
            Assert.True(header.Duplicate);
        }

    }
}
