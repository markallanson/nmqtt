using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Nmqtt;

namespace NmqttTests.Messages.Subscribe
{
    public class Api
    {
        [Fact]
        public void AddSubscriptionOverExistingSubscriptionUpdatesQos()
        {
            MqttSubscribeMessage msg = new MqttSubscribeMessage();
            msg.Payload.AddSubscription("A/Topic", MqttQos.AtMostOnce);
            msg.Payload.AddSubscription("A/Topic", MqttQos.AtLeastOnce);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, msg.Payload.Subscriptions["A/Topic"]);
        }

        [Fact]
        public void ClearSubscriptionsClearsSubscriptions()
        {
            MqttSubscribeMessage msg = new MqttSubscribeMessage();
            msg.Payload.AddSubscription("A/Topic", MqttQos.AtMostOnce);
            msg.Payload.ClearSubscriptions();
            Assert.Equal<int>(0, msg.Payload.Subscriptions.Count);
        }
    }
}
