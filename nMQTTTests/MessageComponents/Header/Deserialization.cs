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
using System.Linq;
using System.Text;
using Xunit;
using Nmqtt;
using System.Reflection;
using System.IO;

namespace NmqttTests.MessageComponents.Header
{
    public class Deserialization
    {
        /// <summary>
        /// Ensures that header can survive the serialization->deserialization round trip without corruption.
        /// </summary>
        [Fact]
        public void HeaderRoundtrip()
        {
            MqttHeader inputHeader = new MqttHeader()
            {
                Duplicate = true,
                Retain = false,
                MessageSize = 1,
                MessageType = MqttMessageType.Connect,
                Qos = MqttQos.AtLeastOnce
            };

            MqttHeader outputHeader;

            using (MemoryStream stream = new MemoryStream())
            {
                inputHeader.WriteTo(0, stream);

                // the stream will be chock full-o-bytes, rewind it so we can read it back
                stream.Seek(0, SeekOrigin.Begin);

                outputHeader = new MqttHeader(stream);
            }

            Assert.Equal<bool>(inputHeader.Duplicate, outputHeader.Duplicate);
            Assert.Equal<bool>(inputHeader.Retain, outputHeader.Retain);
            Assert.Equal<MqttQos>(inputHeader.Qos, outputHeader.Qos);
            Assert.Equal<MqttMessageType>(inputHeader.MessageType, outputHeader.MessageType);
        }

        /// <summary>
        /// Ensures a header with an invalid message size portion is caught and thrown correctly.
        /// </summary>
        [Fact]
        public void CorruptHeader_InvalidMessageSize()
        {
            MqttHeader inputHeader = new MqttHeader()
            {
                Duplicate = true,
                Retain = false,
                MessageSize = 268435455, // the max message size, which we will fudge later in the test
                MessageType = MqttMessageType.Connect,
                Qos = MqttQos.AtLeastOnce
            };

            MqttHeader outputHeader;

            using (MemoryStream stream = new MemoryStream())
            {
                inputHeader.WriteTo(268435455, stream);

                // fudge the header by making the last bit of the 4th message size byte a 1, therefore making the header
                // invalid because the last bit of the 4th size byte should always be 0 (according to the spec). It's how 
                // we know to stop processing the header when reading a full message).
                stream.Seek(4, SeekOrigin.Begin);
                byte existingByte = (byte)stream.ReadByte();
                stream.Seek(4, SeekOrigin.Begin);
                stream.WriteByte((byte)(existingByte | 0xFF));
                stream.Seek(0, SeekOrigin.Begin);

                Assert.Throws<InvalidHeaderException>(() => outputHeader = new MqttHeader(stream));
            }
        }

        /// <summary>
        /// Ensures a header with an invalid message size portion is caught and thrown correctly.
        /// </summary>
        [Fact]
        public void CorruptHeader_Undersize()
        {
            MqttHeader outputHeader;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.WriteByte(0);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<InvalidHeaderException>(() => outputHeader = new MqttHeader(stream));
            }
        }

        [Fact]
        public void Qos_AtMostOnce()
        {
            var headerBytes = GetHeaderBytes(1, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
        }

        [Fact]
        public void Qos_AtLeastOnce()
        {
            var headerBytes = GetHeaderBytes(2, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, header.Qos);
        }

        [Fact]
        public void Qos_ExactlyOnce()
        {
            var headerBytes = GetHeaderBytes(4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, header.Qos);
        }

        [Fact]
        public void Qos_Reserved1()
        {
            var headerBytes = GetHeaderBytes(6, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.Reserved1, header.Qos);
        }


        [Fact]
        public void MessageType_Reserved1()
        {
            var headerBytes = GetHeaderBytes(0, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Reserved1, header.MessageType);
        }

        [Fact]
        public void MessageType_Connect()
        {
            var headerBytes = GetHeaderBytes(1 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Connect, header.MessageType);
        }


        [Fact]
        public void MessageType_ConnectAck()
        {
            var headerBytes = GetHeaderBytes(2 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, header.MessageType);
        }

        [Fact]
        public void MessageType_Publish()
        {
            var headerBytes = GetHeaderBytes(3 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Publish, header.MessageType);
        }

        [Fact]
        public void MessageType_PublishAck()
        {
            var headerBytes = GetHeaderBytes(4 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishAck, header.MessageType);
        }

        [Fact]
        public void MessageType_PublishReceived()
        {
            var headerBytes = GetHeaderBytes(5 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishReceived, header.MessageType);
        }


        [Fact]
        public void MessageType_PublishRelease()
        {
            var headerBytes = GetHeaderBytes(6 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishRelease, header.MessageType);
        }

        [Fact]
        public void MessageType_PublishComplete()
        {
            var headerBytes = GetHeaderBytes(7 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
        }

        [Fact]
        public void MessageType_Subscribe()
        {
            var headerBytes = GetHeaderBytes(8 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Subscribe, header.MessageType);
        }

        [Fact]
        public void MessageType_SubscriptionAck()
        {
            var headerBytes = GetHeaderBytes(9 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.SubscribeAck, header.MessageType);
        }

        [Fact]
        public void MessageType_Unsubscribe()
        {
            var headerBytes = GetHeaderBytes(10 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Unsubscribe, header.MessageType);
        }

        [Fact]
        public void MessageType_UnsubsriptionAck()
        {
            var headerBytes = GetHeaderBytes(11 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.UnsubscribeAck, header.MessageType);
        }

        [Fact]
        public void MessageType_PingRequest()
        {
            var headerBytes = GetHeaderBytes(12 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PingRequest, header.MessageType);
        }

        [Fact]
        public void MessageType_PingResponse()
        {
            var headerBytes = GetHeaderBytes(13 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PingResponse, header.MessageType);
        }

        [Fact]
        public void MessageType_Disconnect()
        {
            var headerBytes = GetHeaderBytes(14 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Disconnect, header.MessageType);
        }

        [Fact]
        public void Duplicate_True()
        {
            var headerBytes = GetHeaderBytes(8, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.True(header.Duplicate);
        }

        [Fact]
        public void Duplicate_False()
        {
            var headerBytes = GetHeaderBytes(0, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.False(header.Duplicate);
        }

        [Fact]
        public void Retain_True()
        {
            var headerBytes = GetHeaderBytes(1, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.True(header.Retain);
        }

        [Fact]
        public void Retain_False()
        {
            var headerBytes = GetHeaderBytes(0, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.False(header.Retain);
        }

        /// <summary>
        /// Creates byte array header with a single byte length
        /// </summary>
        /// <param name="byte1">the first header byte</param>
        /// <param name="length">the length byte</param>
        /// <returns></returns>
        private byte[] GetHeaderBytes(byte byte1, byte length)
        {
            return new [] { byte1, length };
        }

        /// <summary>
        /// Gets the MQTT header from a byte arrayed header.
        /// </summary>
        /// <param name="headerBytes">The header bytes.</param>
        /// <returns></returns>
        private MqttHeader GetMqttHeader(byte[] headerBytes)
        {
            using (MemoryStream stream = new MemoryStream(headerBytes))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return new MqttHeader(stream);
            }
        }
    }
}
