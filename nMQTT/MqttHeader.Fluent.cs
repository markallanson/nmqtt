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

namespace Nmqtt
{
    public partial class MqttHeader
    {
        /*
         * This here class contains some experimental Fluent interface for building
         * an MqttHeader. This is a curiosity at the moment.
         */

        /// <summary>
        /// Sets the type of the message identified in the header.
        /// </summary>
        /// <param name="messageType">The type of message.</param>
        /// <returns>An instance of the header.</returns>
        public MqttHeader AsType(MqttMessageType messageType)
        {
            this.MessageType = messageType;
            return this;
        }

        /// <summary>
        /// Sets the Qos of the message header.
        /// </summary>
        /// <param name="qos">The Qos to ser</param>
        /// <returns>An instance of the header.</returns>
        public MqttHeader WithQos(MqttQos qos)
        {
            this.Qos = qos;
            return this;
        }

        /// <summary>
        /// Sets the IsDuplicate flag of the header.
        /// </summary>
        /// <returns>An Instance of the header.</returns>
        public MqttHeader IsDuplicate()
        {
            this.Duplicate = true;
            return this;
        }

        /// <summary>
        /// Defines that the message should be retained.
        /// </summary>
        /// <returns>An instance of the header,</returns>
        public MqttHeader ShouldBeRetained()
        {
            this.Retain = true;
            return this;
        }
    }
}
