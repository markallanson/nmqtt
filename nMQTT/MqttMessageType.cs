using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    /// <summary>
    /// An enumeration of all available MQTT Message Types
    /// </summary>
    public enum MqttMessageType
    {
        /// <summary>
        /// Reserved by the MQTT spec, should not be used.
        /// </summary>
        Reserved1 = 0,
        Connect = 1,
        ConnectAck,
        Publish,
        PublishAck,
        PublishReceived,
        PublishRelease,
        PublishComplete,
        Subscribe,
        SubscriptionAck,
        Unsubscribe,
        UnsubsriptionAck,
        PingRequest,
        PingResponse,
        Disconnect,

        /// <summary>
        /// Reserved by the MQTT spec, should not be used.
        /// </summary>
        Reserved2
    }
}
