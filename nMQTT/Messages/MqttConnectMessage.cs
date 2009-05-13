using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nMqtt
{
    public class MqttConnectMessage : MqttMessage
    {
        public MqttConnectMessage()
        {
            this.Header = new MqttHeader()
            {
                MessageType = MqttMessageType.Connect
            };

            this.VariableHeader = new MqttConnectVariableHeader()
            {
                ConnectFlags = new MqttConnectFlags()
                {
                     
                }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectMessage"/> class.
        /// </summary>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttConnectMessage(Stream messageStream)
        {
            this.VariableHeader = new MqttConnectVariableHeader(messageStream);
            this.Payload = new MqttConnectPayload(messageStream, VariableHeader.ConnectFlags.WillFlag);
        }
    }
}
