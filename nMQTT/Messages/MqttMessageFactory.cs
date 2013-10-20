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

using System;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    ///     Factory for generating instances of MQTT Messages
    /// </summary>
    internal static class MqttMessageFactory
    {
        /// <summary>
        ///     Gets an instance of an MqttMessage based on the message type requested.
        /// </summary>
        /// <param name="header">The message header.</param>
        /// <param name="messageStream">The content of the message, including variable header where applicable.</param>
        /// <returns>An instance of the desired message type.</returns>
        public static MqttMessage GetMessage(MqttHeader header, Stream messageStream) {
            switch (header.MessageType) {
                case MqttMessageType.Connect:
                    return new MqttConnectMessage(header, messageStream);
                case MqttMessageType.ConnectAck:
                    return new MqttConnectAckMessage(header, messageStream);
                case MqttMessageType.Publish:
                    return new MqttPublishMessage(header, messageStream);
                case MqttMessageType.PublishAck:
                    return new MqttPublishAckMessage(header, messageStream);
                case MqttMessageType.PublishComplete:
                    return new MqttPublishCompleteMessage(header, messageStream);
                case MqttMessageType.PublishReceived:
                    return new MqttPublishReceivedMessage(header, messageStream);
                case MqttMessageType.PublishRelease:
                    return new MqttPublishReleaseMessage(header, messageStream);
                case MqttMessageType.Subscribe:
                    return new MqttSubscribeMessage(header, messageStream);
                case MqttMessageType.SubscribeAck:
                    return new MqttSubscribeAckMessage(header, messageStream);
                case MqttMessageType.Unsubscribe:
                    return new MqttUnsubscribeMessage(header, messageStream);
                case MqttMessageType.UnsubscribeAck:
                    return new MqttUnsubscribeAckMessage(header, messageStream);
                case MqttMessageType.PingRequest:
                    return new MqttPingRequestMessage(header);
                case MqttMessageType.PingResponse:
                    return new MqttPingResponseMessage(header);
                case MqttMessageType.Disconnect:
                    return new MqttDisconnectMessage(header);
                default:
                    throw new InvalidHeaderException(
                        String.Format(
                            "The Message Type specified ({0}) is not a valid MQTT Message type or currently not supported.",
                            (int) header.MessageType));
            }
        }
    }
}