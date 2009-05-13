using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using nMqtt.ExtensionMethods;

namespace nMqtt
{
    /// <summary>
    /// Class that contains details related to an MQTT Connect messages payload 
    /// </summary>
    public class MqttConnectPayload : MqttPayload
    {
        private bool willFlag;

        private string clientIdentifier;
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
        {
            this.willFlag = willFlag;

            ReadFrom(payloadStream);
        }

        public override void ReadFrom(Stream payloadStream)
        {
            ClientIdentifier = payloadStream.ReadMqttString();

        }
    }
}
