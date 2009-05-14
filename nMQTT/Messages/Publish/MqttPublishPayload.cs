using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Nmqtt.ExtensionMethods;
using System.Collections.ObjectModel;

namespace Nmqtt
{
    /// <summary>
    /// Class that contains details related to an MQTT Connect messages payload 
    /// </summary>
    public sealed class MqttPublishPayload : MqttPayload
    {
        private MqttHeader header;
        private MqttPublishVariableHeader variableHeader;


        private Collection<byte> message;

        /// <summary>
        /// The message that forms the payload of the publish message.
        /// </summary>
        public Collection<byte> Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectPayload"/> class.
        /// </summary>
        public MqttPublishPayload()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectPayload"/> class.
        /// </summary>
        /// <param name="header">The header of the message being process.</param>
        /// <param name="variableHeader">The variable header of the message being processed.</param>
        /// <param name="payloadStream">The payload stream.</param>
        public MqttPublishPayload(MqttHeader header, MqttPublishVariableHeader variableHeader, Stream payloadStream)
        {
            this.header = header;
            this.variableHeader = variableHeader;
            ReadFrom(payloadStream);
        }

        /// <summary>
        /// Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void ReadFrom(Stream payloadStream)
        {
            // The payload of the publish message is not a string, just a binary chunk of bytes.
            // The length of the bytes is the length specified in the header, minus any bytes 
            // spent in the variable header.

            var messageBytes = new byte[header.MessageSize - variableHeader.Length];
            int messageBytesRead = payloadStream.Read(messageBytes, 0, messageBytes.Length);
            message = new Collection<byte>(messageBytes);

            // TODO: If we're processing a raw TCP stream, we might want to go into a loop here to keep reading bytes until we have read the amount we expect.

            // Throw out an exception we don't have enough bytes in the underlying stream.
            if (messageBytesRead < Message.Count)
            {
                throw new InvalidPayloadSizeException(
                    String.Format("The length of data in the payload ({0}) did not match the expected payload size ({1}).",
                        messageBytesRead, Message.Count));
            }
        }

        /// <summary>
        /// Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString()
        {
            return String.Format("Payload: {0} bytes={1}", Message == null ? -1 : Message.Count, BytesToString(Message));
        }

        /// <summary>
        /// Converts an array of bytes to a byte string.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <returns>The message as an array of bytes</returns>
        private static string BytesToString(IEnumerable<byte> Message)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in Message)
            {
                sb.Append('<');
                sb.Append(b);
                sb.Append('>');
            }
            return sb.ToString();
        }
    }
}
