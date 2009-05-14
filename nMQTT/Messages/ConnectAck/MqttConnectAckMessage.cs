using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    public sealed class MqttConnectAckMessage : MqttMessage
    {
        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttConnectAckVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectAckMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttConnectAckMessage()
        {
            this.Header = new MqttHeader()
            {
                MessageType = MqttMessageType.ConnectAck
            };

            this.VariableHeader = new MqttConnectAckVariableHeader()
            {
                ConnectFlags = new MqttConnectFlags()
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectAckMessage"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttConnectAckMessage(MqttHeader header, Stream messageStream)
        {
            this.Header = header;
            this.VariableHeader = new MqttConnectAckVariableHeader(messageStream);
            // no payload in connectack messages
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(base.ToString());
            sb.AppendLine(VariableHeader.ToString());

            return sb.ToString();
        }

    }
}
