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
        private MqttMessage()
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
        public static MqttMessage Create(IEnumerable<byte> messageBytes)
        {
            using (MemoryStream messageStream = new MemoryStream(messageBytes.ToArray<byte>()))
            {
                return Create(messageStream);
            }
        }

        /// <summary>
        /// Creates a new instance of an MQTT Message based on a raw message stream.
        /// </summary>
        /// <param name="messageStream">The message stream.</param>
        /// <returns>An MqttMessage containing details of the message.</returns>
        public static MqttMessage Create(Stream messageStream)
        {
            MqttMessage message = new MqttMessage();

            // pass the input stream sequentially through the component deserialization(create) methods
            // to build a full MqttMessage.
            message.Header = MqttHeader.Create(messageStream);

            // TODO: Implement the same for variable header and Payload

            return message;
        }

        /// <summary>
        /// Gets the raw message bytes of the message. these bytes are the actual raw MQTT message.
        /// </summary>
        /// <value>The message bytes.</value>
        internal List<byte> MessageBytes
        {
            get
            {
                var messageBytes = new List<byte>();

                messageBytes.AddRange(Header.HeaderBytes);

                return messageBytes;
            }
        }
    }
}
