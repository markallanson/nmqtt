/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System.IO;

namespace Nmqtt
{
    /// <summary>
    ///     Represents the payload (Body) of an MQTT Message.
    /// </summary>
    internal abstract class MqttPayload
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttPayload" /> class.
        /// </summary>
        protected MqttPayload() {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttPayload" /> class.
        /// </summary>
        /// <param name="payloadStream">The payload stream.</param>
        protected MqttPayload(Stream payloadStream) {
            ReadFrom(payloadStream);
        }

        /// <summary>
        ///     Writes the payload to the supplied stream.
        /// </summary>
        /// <param name="payloadStream">The stream to write the variable header to.</param>
        /// <remarks>
        ///     A basic message has no Variable Header.
        /// </remarks>
        public abstract void WriteTo(Stream payloadStream);

        /// <summary>
        ///     Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="payloadStream">The stream to read the payload from.</param>
        public abstract void ReadFrom(Stream payloadStream);

        /// <summary>
        ///     Gets the length of the payload in bytes when written to a stream.
        /// </summary>
        /// <returns>The length of the payload in bytes.</returns>
        internal abstract int GetWriteLength();
    }
}