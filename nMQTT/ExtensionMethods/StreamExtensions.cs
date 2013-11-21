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

using System;
using System.Collections;
using System.IO;
using Nmqtt.Encoding;

namespace Nmqtt.ExtensionMethods
{
    /// <summary>
    ///     Provides stream extension methods useful for interacting with streams of MQTT messges.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        ///     Writes a short to the underlying stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write to the stream.</param>
        public static void WriteShort(this Stream stream, short value) {
            stream.WriteByte((byte) (value >> 8));
            stream.WriteByte((byte) (value & 0xFF));
        }

        /// <summary>
        ///     Reads a short from the underlying stream.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>A short value.</returns>
        public static short ReadShort(this Stream stream) {
            byte high, low;
            high = (byte) stream.ReadByte();
            low = (byte) stream.ReadByte();

            return (short) ((high << 8) + low);
        }

        /// <summary>
        ///     Reads an MQTT string from the underlying stream.
        /// </summary>
        /// <param name="stringStream">The stream to read the string from.</param>
        /// <returns>The Mqtt String.</returns>
        public static string ReadMqttString(this Stream stringStream)
        {
            // read and check the length
            var lengthBytes = new byte[2];
            var bytesRead = stringStream.Read(lengthBytes, 0, 2);
            if (bytesRead < 2)
            {
                throw new ArgumentException(
                    "The stream did not have enough bytes to describe the length of the string",
                    "stringStream");
            }

            System.Text.Encoding enc = new MqttEncoding();
            var stringLength = (ushort)enc.GetCharCount(lengthBytes);

            // read the bytes from the string, validate we have enough etc.
            var stringBytes = new byte[stringLength];
            var readBuffer = new byte[1 << 10]; // 1KB read buffer
            var totalRead = 0;

            // Keep reading until we have all. Intentionally synchronous
            while (totalRead < stringLength) {
                var remainingBytes = stringLength - totalRead;
                var nextReadSize = remainingBytes > readBuffer.Length ? readBuffer.Length : remainingBytes;
                bytesRead = stringStream.Read(readBuffer, 0, nextReadSize);
                Array.Copy(readBuffer, 0, stringBytes, totalRead, bytesRead);
                totalRead += bytesRead;
            }

            return enc.GetString(stringBytes);
        }

        /// <summary>
        ///     Writes the MQTT string.
        /// </summary>
        /// <param name="stringStream">The stream containing the string to write.</param>
        /// <param name="stringToWrite">The string to write.</param>
        public static void WriteMqttString(this Stream stringStream, string stringToWrite) {
            System.Text.Encoding enc = new MqttEncoding();
            byte[] stringBytes = enc.GetBytes(stringToWrite);
            stringStream.Write(stringBytes, 0, stringBytes.Length);
        }
    }
}