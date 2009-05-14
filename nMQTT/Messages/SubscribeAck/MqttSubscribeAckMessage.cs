using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    public sealed class MqttSubscribeAckMessage : MqttMessage
    {
        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttSubscribeAckVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttSubscribeAckPayload Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttSubscribeAckMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttSubscribeAckMessage()
        {
            this.Header = new MqttHeader().AsType(MqttMessageType.SubscribeAck);

            this.VariableHeader = new MqttSubscribeAckVariableHeader()
            {
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttSubscribeAckMessage"/> class.
        /// </summary>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttSubscribeAckMessage(MqttHeader header, Stream messageStream)
        {
            this.Header = header;
            ReadFrom(messageStream);
        }

        public override void ReadFrom(Stream messageStream)
        {
            this.VariableHeader = new MqttSubscribeAckVariableHeader(messageStream);
            this.Payload = new MqttSubscribeAckPayload(Header, VariableHeader, messageStream);
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
            sb.AppendLine(Payload.ToString());

            return sb.ToString();
        }

    }
}
