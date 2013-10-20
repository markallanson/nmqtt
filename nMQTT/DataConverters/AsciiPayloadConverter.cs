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
    ///     Converts string data to and from the MQTT wire format
    /// </summary>
    public class AsciiPayloadConverter : IPayloadConverter<string>
    {
        /// <summary>
        ///     Processes received data and returns it as a string.
        /// </summary>
        /// <param name="messageData">The received data as an array of bytes.</param>
        /// <returns>
        ///     The data processed and turned into the specified type.
        /// </returns>
        public string ConvertFromBytes(byte[] messageData) {
            return System.Text.Encoding.ASCII.GetString(messageData);
        }

        /// <summary>
        ///     Converts sent data from a string to a byte array.
        /// </summary>
        /// <param name="data">The string to convert to the byte array.</param>
        /// <returns>A byte array representation of the string.</returns>
        public byte[] ConvertToBytes(string data) {
            return System.Text.Encoding.ASCII.GetBytes(data);
        }
    }
}