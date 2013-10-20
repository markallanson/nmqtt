/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

namespace Nmqtt
{
    /// <summary>
    ///     An enumeration of all available MQTT Message Types
    /// </summary>
    internal enum MqttMessageType
    {
        /// <summary>
        ///     Reserved by the MQTT spec, should not be used.
        /// </summary>
        Reserved1 = 0,

        Connect,
        ConnectAck,
        Publish,
        PublishAck,
        PublishReceived,
        PublishRelease,
        PublishComplete,
        Subscribe,
        SubscribeAck,
        Unsubscribe,
        UnsubscribeAck,
        PingRequest,
        PingResponse,
        Disconnect,

        /// <summary>
        ///     Reserved by the MQTT spec, should not be used.
        /// </summary>
        Reserved2
    }
}