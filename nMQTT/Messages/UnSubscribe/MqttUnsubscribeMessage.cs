using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    public sealed class MqttUnsubscribeMessage : MqttMessage
    {
        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttUnsubscribeVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttUnsubscribePayload Payload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttUnsubscribeMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttUnsubscribeMessage()
        {
            this.Header = new MqttHeader().AsType(MqttMessageType.Unsubscribe);

            this.VariableHeader = new MqttUnsubscribeVariableHeader()
            {
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttUnsubscribeMessage"/> class.
        /// </summary>
        /// <param name="messageStream">The message stream positioned after the header.</param>
        internal MqttUnsubscribeMessage(MqttHeader header, Stream messageStream)
        {
            this.Header = header;
            ReadFrom(messageStream);
        }

        public override void ReadFrom(Stream messageStream)
        {
            this.VariableHeader = new MqttUnsubscribeVariableHeader(messageStream);
            this.Payload = new MqttUnsubscribePayload(Header, VariableHeader, messageStream);
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
