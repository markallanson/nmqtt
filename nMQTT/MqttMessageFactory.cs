using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nMqtt
{
    /// <summary>
    /// Factory for generating instances of MQTT Messages
    /// </summary>
    internal static class MqttMessageFactory
    {
        /// <summary>
        /// Gets an instance of an MqttMessage based on the message type requested.
        /// </summary>
        /// <param name="messageType">Type of message to retrieve an instance of.</param>
        /// <returns>An instance of the desired message type.</returns>
        public static MqttMessage GetMessage(MqttHeader header, Stream messageStream)
        {
            MqttMessage message;

            switch (header.MessageType)
            {
                case MqttMessageType.Connect:
                    message = new MqttConnectMessage(messageStream);
                    break;
                default:
                    throw new InvalidHeaderException(
                        String.Format("The Message Type specified ({0}) is not a valid MQTT Message type.", (int)header.MessageType));
            }

            // give it the header we read originally
            message.Header = header;

            return message;
        }
    }
}
