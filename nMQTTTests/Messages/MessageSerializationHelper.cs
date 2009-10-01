using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmqtt;
using System.IO;

namespace NmqttTests.Messages
{
    /// <summary>
    /// Helper methods for test message serialization and deserialization 
    /// </summary>
    internal static class MessageSerializationHelper
    {
        /// <summary>
        /// Invokes the serialization of a message to get an array of bytes that represent the message.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        internal static byte[] GetMessageBytes(MqttMessage msg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                msg.WriteTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] msgBytes = new byte[ms.Length];
                ms.Read(msgBytes, 0, (int)ms.Length);

                return msgBytes;
            }
        }
    }
}
