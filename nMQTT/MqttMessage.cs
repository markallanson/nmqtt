using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nMqtt
{
    /// <summary>
    /// Represents an MQTT message that contains a fixed header, variable header and message body.
    /// </summary>
    /// <remarks>
    /// Messages roughly look as follows.
    /// <code>
    /// ----------------------------
    /// | Header, 2-5 Bytes Length |
    /// ----------------------------
    /// | Variable Header          |
    /// | n Bytes Length           |
    /// ----------------------------
    /// | Message Payload          |
    /// | 256MB minus VH Size      |
    /// ----------------------------
    /// </code>
    /// </remarks>
    public class MqttMessage
    {
        /// <summary>
        /// The header of the MQTT Message. Contains metadata about the message
        /// </summary>
        public MqttHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the payload of the Mqtt Message.
        /// </summary>
        /// <value>The payload of the Mqtt Message.</value>
        public MqttPayload Payload { get; set; }

        /// <summary>
        /// Gets or sets the variable header contents. Contains extended metadata about the message
        /// </summary>
        /// <value>The variable header.</value>
        public MqttVariableHeader VariableHeader { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttMessage"/> class.
        /// </summary>
        /// <remarks>
        /// Only called via the MqttMessage.Create operation during processing of an Mqtt message stream.
        /// </remarks>
        public MqttMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttMessage"/> class.
        /// </summary>
        /// <param name="header">The header of the message.</param>
        /// <param name="payload">The payload of the message.</param>
        public MqttMessage(MqttHeader header, MqttPayload payload)
        {
            Header = header;
            Payload = payload;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttMessage"/> class.
        /// </summary>
        /// <param name="header">The header of the message.</param>
        /// <param name="payload">The payload of the message.</param>
        /// <param name="variableHeader">The variable header of the message.</param>
        public MqttMessage(MqttHeader header, MqttPayload payload, MqttVariableHeader variableHeader)
            : this(header, payload)
        {
            VariableHeader = variableHeader;
        }

        /// <summary>
        /// Creates a new instance of an MQTT Message based on a raw message bytes.
        /// </summary>
        /// <param name="messageBytes">The message bytes.</param>
        /// <returns></returns>
        public static MqttMessage CreateFrom(IEnumerable<byte> messageBytes)
        {
            using (MemoryStream messageStream = new MemoryStream(messageBytes.ToArray<byte>()))
            {
                return CreateFrom(messageStream);
            }
        }

        /// <summary>
        /// Creates a new instance of an MQTT Message based on a raw message stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        /// <returns>An MqttMessage containing details of the message.</returns>
        public static MqttMessage CreateFrom(Stream messageStream)
        {
            MqttHeader header = new MqttHeader();

            // pass the input stream sequentially through the component deserialization(create) methods
            // to build a full MqttMessage.
            header = new MqttHeader(messageStream);

            MqttMessage message = MqttMessageFactory.GetMessage(header, messageStream);

            return message;
        }

        /// <summary>
        /// Writes the message to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to write the message to.</param>
        public void WriteTo(Stream messageStream)
        {
            Header.WriteTo(messageStream);
            VariableHeader.WriteTo(messageStream);
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

            sb.Append("MQTTMessage of type ");
            sb.AppendLine(this.GetType().ToString());

            sb.Append(Header.ToString());

            if (VariableHeader != null)
            {
                sb.Append(VariableHeader.ToString());
            }

            if (Payload != null)
            {
                sb.Append(Payload.ToString());
            }

            return sb.ToString();
        }
    }
}
