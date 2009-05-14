/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

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
    /// Class that contains details related to an MQTT SubscribeAck messages payload 
    /// </summary>
    public sealed class MqttSubscribeAckPayload : MqttPayload
    {
        private MqttVariableHeader variableHeader;
        private MqttHeader header;

        private Collection<MqttQos> qosGrants = new Collection<MqttQos>();

        /// <summary>
        /// The collection of subscriptions, Key is the topic, Value is the qos
        /// </summary>
        public Collection<MqttQos> QosGrants
        {
            get
            {
                return qosGrants;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttSubscribeAckPayload"/> class.
        /// </summary>
        public MqttSubscribeAckPayload()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttSubscribeAckPayload"/> class.
        /// </summary>
        /// <param name="payloadStream">The payload stream.</param>
        /// <param name="willFlag">
        /// Set to true to indicate that the payload stream should be interrogated for the 
        /// Will Topic and Message</param>
        public MqttSubscribeAckPayload(MqttHeader header, MqttSubscribeAckVariableHeader variableHeader, Stream payloadStream)
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
            int payloadBytesRead = 0;
            int payloadLength = header.MessageSize - variableHeader.Length;

            // read all the topics and qos subscriptions from the message payload
            while (payloadBytesRead < payloadLength)
            {
                MqttQos granted = (MqttQos)payloadStream.ReadByte();
                payloadBytesRead++; 
                AddGrant(granted);
            }
        }

        /// <summary>
        /// Adds a new subscription to the collection of subscriptions.
        /// </summary>
        /// <param name="grantedQos">The granted qos.</param>
        public void AddGrant(MqttQos grantedQos)
        {
            qosGrants.Add(grantedQos);
        }

        /// <summary>
        /// Clears the subscriptions.
        /// </summary>
        public void ClearGrants()
        {
            qosGrants.Clear();
        }

        /// <summary>
        /// Returns a string representation of the payload.
        /// </summary>
        /// <returns>A string representation of the payload.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("Payload: Subscription Grants [{0}]", qosGrants.Count));

            foreach (MqttQos grant in qosGrants)
            {
                sb.AppendLine(String.Format("{{ Grant={0} }}", grant));
            }

            return sb.ToString();
        }
    }
}
