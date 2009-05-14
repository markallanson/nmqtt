using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Implementation of the variable header for an MQTT Publish Acknowledgement message.
    /// </summary>
    public sealed class MqttUnsubscribeAckVariableHeader : MqttVariableHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        public MqttUnsubscribeAckVariableHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttUnsubscribeAckVariableHeader(Stream headerStream)
        {
            ReadFrom(headerStream);
        }

        /// <summary>
        /// Returns the read flags for the publish message (topic, messageid)
        /// </summary>
        protected override ReadWriteFlags ReadFlags
        {
            get
            {
                return ReadWriteFlags.MessageIdentifier;
            }
        }

        /// <summary>
        /// Returns the read flags for the publish message (topic, messageid)
        /// </summary>
        protected override ReadWriteFlags WriteFlags
        {
            get
            {
                // Read and write flags are identical for Publish Messages
                return ReadFlags;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return
                String.Format("UnsubscribeAck Variable Header: MessageIdentifier={0}", MessageIdentifier);
        }
    }
}
