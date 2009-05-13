using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using nMqtt.Encoding;

namespace nMqtt.ExtensionMethods
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
        public static string ReadMqttString(this Stream stream)
        {
                int stringPosition = (int)stream.Position;

                // read and check the length
                var lengthBytes = new byte[2];
                int bytesRead = stream.Read(lengthBytes, 0, 2);
                if (bytesRead < 2)
                {
                    throw new ArgumentException("stream", 
                        String.Format("The stream did not have enough bytes to describe the length of the string at position {0}", stream.Position));
                }

                System.Text.Encoding enc = new MqttEncoding();
                short stringLength = (short)enc.GetCharCount(lengthBytes);

                // read the bytes from the string, validate we have enough etc.
                var stringBytes = new byte[stringLength];
                bytesRead = stream.Read(stringBytes, 0, stringLength);
                if (bytesRead < stringLength)
                {
                    throw new ArgumentException("stream", 
                        String.Format("The stream did not have enough bytes to match the defined string length {0} defined at stream position {1}", stringLength, stream.Position));
                }

                return enc.GetString(stringBytes);
        }

        /// <summary>
        /// Writes the MQTT string.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="stringToWrite">The string to write.</param>
        public static void WriteMqttString(this Stream stream, string stringToWrite)
        {
            System.Text.Encoding enc = new MqttEncoding();
            byte[] stringBytes = enc.GetBytes(stringToWrite);
            stream.Write(stringBytes, 0, stringBytes.Length);
        }
    }
}
