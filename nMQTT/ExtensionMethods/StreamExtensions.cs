/* 
 * nMQTT, a .Net MQTT v3 client implementation.
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
using Nmqtt.Encoding;

namespace Nmqtt.ExtensionMethods
{
    /// <summary>
    /// Provides stream extension methods useful for interacting with streams of MQTT messges.
    /// </summary>
    internal static class StreamExtensions
    {
        /// <summary>
        /// Writes a short to the underlying stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="value">The value to write to the stream.</param>
        public static void WriteShort(this Stream stream, short value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)(value & 0xFF));
        }

        /// <summary>
        /// Reads a short from the underlying stream.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>A short value.</returns>
        public static short ReadShort(this Stream stream)
        {
            byte high, low;
            high = (byte)stream.ReadByte();
            low = (byte)stream.ReadByte();

            return (short)((high << 8) + low);
        }

        /// <summary>
        /// Reads an MQTT string from the underlying stream.
        /// </summary>
        /// <param name="stream">The stream to read the string from.</param>
        /// <returns>The Mqtt String.</returns>
        public static string ReadMqttString(this Stream stringStream)
        {
            // read and check the length
            var lengthBytes = new byte[2];
            int bytesRead = stringStream.Read(lengthBytes, 0, 2);
            if (bytesRead < 2)
            {
                throw new ArgumentException(
                    "The stream did not have enough bytes to describe the length of the string",
                    "stringStream");
            }

            System.Text.Encoding enc = new MqttEncoding();
            short stringLength = (short)enc.GetCharCount(lengthBytes);

            // read the bytes from the string, validate we have enough etc.
            var stringBytes = new byte[stringLength];
            bytesRead = stringStream.Read(stringBytes, 0, stringLength);
            if (bytesRead < stringLength)
            {
                throw new ArgumentException("stream",
                    String.Format("The stream did not have enough bytes to match the defined string length {0}", stringLength));
            }

            return enc.GetString(stringBytes);
        }

        /// <summary>
        /// Writes the MQTT string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="stringToWrite">The string to write.</param>
        public static void WriteMqttString(this Stream stringStream, string stringToWrite)
        {
            System.Text.Encoding enc = new MqttEncoding();
            byte[] stringBytes = enc.GetBytes(stringToWrite);
            stringStream.Write(stringBytes, 0, stringBytes.Length);
        }
    }
}
