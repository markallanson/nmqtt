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

namespace Nmqtt
{
    /// <summary>
    /// Represents the payload (Body) of an MQTT Message.
    /// </summary>
    public abstract class MqttPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MqttPayload"/> class.
        /// </summary>
        protected MqttPayload()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttPayload"/> class.
        /// </summary>
        /// <param name="payloadStream">The payload stream.</param>
        protected MqttPayload(Stream payloadStream)
        {
            ReadFrom(payloadStream);
        }

        /// <summary>
        /// Writes the payload to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to s the variable header to.</param>
        /// <remarks>
        /// A basic message has no Variable Header.
        /// </remarks>
        public abstract void WriteTo(Stream payloadStream);

        /// <summary>
        /// Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="headerStream">The header stream.</param>
        public abstract void ReadFrom(Stream payloadStream);

        /// <summary>
        /// Gets the length of the payload in bytes when written to a stream.
        /// </summary>
        /// <returns>The length of the payload in bytes.</returns>
        internal abstract int GetWriteLength();

        public override string ToString()
        {
            return "Payload: (none)";
        }
    }
}
