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
using System.Collections.Generic;
using System.Text;

namespace Nmqtt.Encoding
{
    /// <summary>
    ///     Encoding implementation that can encode and decode strings in the MQTT string format.
    /// </summary>
    /// <remarks>
    ///     The MQTT string format is simply a pascal string with ANSI character encoding. The first 2 bytes define
    ///     the length of the string, and they are followed by the string itself.
    /// </remarks>
    internal class MqttEncoding : ASCIIEncoding
    {
        /// <summary>
        ///     When overridden in a derived class, encodes all the characters in the specified <see cref="T:System.String" /> into a sequence of bytes.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>
        ///     A byte array containing the results of encoding the specified set of characters.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="s" /> is null.
        /// </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">
        ///     A fallback occurred (see Understanding Encodings for complete explanation)
        ///     -and-
        ///     <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.
        /// </exception>
        public override byte[] GetBytes(string s) {
            ValidateString(s);

            var stringBytes = new List<byte>();
            stringBytes.Add((byte) (s.Length >> 8));
            stringBytes.Add((byte) (s.Length & 0xFF));
            stringBytes.AddRange(ASCII.GetBytes(s));

            return stringBytes.ToArray();
        }

        /// <summary>
        ///     When overridden in a derived class, decodes all the bytes in the specified byte array into a string.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <returns>
        ///     A <see cref="T:System.String" /> containing the results of decoding the specified sequence of bytes.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="bytes" /> is null.
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">
        ///     A fallback occurred (see Understanding Encodings for complete explanation)
        ///     -and-
        ///     <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.
        /// </exception>
        public override string GetString(byte[] bytes) {
            return ASCII.GetString(bytes);
        }

        /// <summary>
        ///     When overridden in a derived class, calculates the number of characters produced by decoding all the bytes in the specified byte array.
        /// </summary>
        /// <param name="bytes">The byte array containing the sequence of bytes to decode.</param>
        /// <returns>
        ///     The number of characters produced by decoding the specified sequence of bytes.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="bytes" /> is null.
        /// </exception>
        /// <exception cref="T:System.Text.DecoderFallbackException">
        ///     A fallback occurred (see Understanding Encodings for complete explanation)
        ///     -and-
        ///     <see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.
        /// </exception>
        public override int GetCharCount(byte[] bytes) {
            if (bytes.Length < 2) {
                throw new ArgumentException("Length byte array must comprise 2 bytes");
            }

            return (ushort) ((bytes[0] << 8) + bytes[1]);
        }

        /// <summary>
        ///     Calculates the number of bytes produced by encoding the characters in the specified <see cref="T:System.String" />.
        /// </summary>
        /// <param name="chars">
        ///     The <see cref="T:System.String" /> containing the set of characters to encode.
        /// </param>
        /// <returns>
        ///     The number of bytes produced by encoding the specified characters.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="chars" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     The resulting number of bytes is greater than the maximum number that can be returned as an integer.
        /// </exception>
        /// <exception cref="T:System.Text.EncoderFallbackException">
        ///     A fallback occurred (see Understanding Encodings for complete explanation)
        ///     -and-
        ///     <see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.
        /// </exception>
        public override int GetByteCount(string chars) {
            ValidateString(chars);
            return System.Text.Encoding.ASCII.GetByteCount(chars) + 2;
        }

        /// <summary>
        ///     Validates the string to ensure it doesn't contain any characters invalid within the Mqtt string format.
        /// </summary>
        /// <param name="s">The s.</param>
        private static void ValidateString(string s) {
            foreach (var c in s) {
                if (c > 0x7F) {
                    throw new ArgumentException("The input string has extended UTF characters, which are not supported");
                }
            }
        }
    }
}