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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Event arguments for the publishing of messages received from a message broker.
    /// </summary>
    public class PublishEventArgs : EventArgs
    {
        private Collection<byte> message;

        /// <summary>
        /// Gets or sets the Message that has been published.
        /// </summary>
        /// <value>The Message that has been published.</value>
        public Collection<byte> Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="MessageStream">The Message that has been published.</param>
        public PublishEventArgs(Collection<byte> message)
        {
            this.message = message;
        }

        /// <summary>
        /// Gets as Message as a string, using the Utf-8 encoding to decode the message.
        /// </summary>
        /// <returns>The message as a string.</returns>
        public string GetAsUtf8String()
        {
            return GetAsString(System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Gets as Message as a string, using the Ascii encoding to decode the message.
        /// </summary>
        /// <returns>The message as a string.</returns>
        public string GetAsAsciiString()
        {
            return GetAsString(System.Text.Encoding.ASCII);
        }

        /// <summary>
        /// Gets as Message as a string, using the Mqtt String format encoding to decode the message.
        /// </summary>
        /// <returns>The message as a string.</returns>
        public string GetAsMqttString()
        {
            return GetAsString(new Nmqtt.Encoding.MqttEncoding());
        }

        /// <summary>
        /// Retreives the first byte of the message.
        /// </summary>
        /// <returns>The first byte of the message.</returns>
        public byte GetAsByte()
        {
            if (message.Count == 0)
            {
                throw new FormatException("The message data does not contain a byte");
            }

            return message[0];
        }

        /// <summary>
        /// Retreives the first short in the message.
        /// </summary>
        /// <returns>The first short in the message.</returns>
        public short GetAsShort()
        {
            if (message.Count < 2)
            {
                throw new FormatException("The message data does not contain enough data to extract a short.");
            }
            return (short)(message[0] << 8 + message[1]);
        }

        /// <summary>
        /// Retreives the first integer in the message.
        /// </summary>
        /// <returns>The first integer in the message.</returns>
        public int GetAsInt()
        {
            if (message.Count < 4)
            {
                throw new FormatException("The message data does not contain enough data to extract a 32 bit integer.");
            }

            return message[0] << 32 + message[1] << 16 + message[2] << 8 + message[3];
        }

        /// <summary>
        /// Retreives the first long (64bit integer) in the message.
        /// </summary>
        /// <returns>The first long in the message.</returns>
        public long GetAsLong()
        {
            if (message.Count < 4)
            {
                throw new FormatException("The message data does not contain enough data to extract a 64 bit integer.");
            }

            return (long)
                (
                (long)(message[0] << 512) + 
                (long)(message[1] << 256) + 
                (long)(message[2] << 128) + 
                (long)(message[3] << 64) + 
                (long)(message[4] << 32) + 
                (long)(message[5] << 16) + 
                (long)(message[6] << 8) + 
                (long)(message[7])
                );
        }

        /// <summary>
        /// Gets the message a string of specified encoding
        /// </summary>
        /// <param name="encoding">The encoding to use to decode the message into a string.</param>
        /// <returns>The message data as a string.</returns>
        public string GetAsString(System.Text.Encoding encoding)
        {
            return encoding.GetString(message.ToArray<byte>());
        }
    }
}
