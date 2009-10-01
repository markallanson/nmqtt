using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmqtt;
using Xunit;

namespace NmqttTests.Messages.Publish
{
    public class Fluent
    {
        [Fact]
        public void ClearPublishData()
        {
            // set up some publish data
            MqttPublishMessage msg = new MqttPublishMessage()
                .PublishData(new[] { (byte)0, (byte)1 });

            Assert.Equal<int>(2, msg.Payload.Message.Count);

            msg.ClearPublishData();

            Assert.Equal<int>(0, msg.Payload.Message.Count);
        }
    }
}
