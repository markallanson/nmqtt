using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    /// Class that contains details related to an MQTT Connect messages payload 
    /// </summary>
    public sealed class MqttConnectPayload : MqttPayload
    {
        private bool willFlag;

        private string clientIdentifier;

        /// <summary>
        /// The identifier of the client that is/has sent the connet message.
        /// </summary>
        public string ClientIdentifier
        {
            get
            {
                return clientIdentifier;
            }
            set
            {
                if (value.Length > Constants.MaxClientIdentifierLength)
                {
                    throw new ClientIdentifierException(value);
                }

                clientIdentifier = value;
            }
        }

        public string WillTopic { get; set; }
        public string WillMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectPayload"/> class.
        /// </summary>
        public MqttConnectPayload()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectPayload"/> class.
        /// </summary>
        /// <param name="payloadStream">The payload stream.</param>
        /// <param name="willFlag">
        /// Set to true to indicate that the payload stream should be interrogated for the 
        /// Will Topic and Message</param>
        public MqttConnectPayload(Stream payloadStream, bool willFlag)
            : base(payloadStream)
        {
            this.willFlag = willFlag;
        }

        /// <summary>
        /// Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream"></param>
        public override void ReadFrom(Stream payloadStream)
        {
            ClientIdentifier = payloadStream.ReadMqttString();

            if (this.willFlag)
            {
                WillTopic = payloadStream.ReadMqttString();
                WillMessage = payloadStream.ReadMqttString();
            }
        }

        /// <summary>
        /// Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString()
        {
            return String.Format("Payload: ClientIdentifier={0}, WillTopic={1}, WillMessage={2}",
                ClientIdentifier, WillTopic, WillMessage);
        }
    }
}
