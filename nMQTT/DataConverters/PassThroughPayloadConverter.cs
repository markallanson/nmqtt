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

namespace Nmqtt
{
    /// <summary>
    ///     Acts as a passthrough for the raw data without doing any conversion.
    /// </summary>
    /// s
    internal class PassThroughPayloadConverter : IPayloadConverter<byte[]>
    {
        /// <summary>
        ///     Processes received data and returns it as a byte array.
        /// </summary>
        /// <param name="messageData">The received data as an array of bytes.</param>
        /// <returns>
        ///     The data processed and turned into a byte array.
        /// </returns>
        public byte[] ConvertFromBytes(byte[] messageData) {
            return messageData;
        }

        /// <summary>
        ///     Converts sent data from an object graph to a byte array.
        /// </summary>
        /// <param name="data">The data to convert to the byte array.</param>
        /// <returns>A byte array representation of the data.</returns>
        public byte[] ConvertToBytes(byte[] data) {
            return data;
        }
    }
}