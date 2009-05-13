using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using nMqtt;
using System.Reflection;
using System.IO;

namespace nMqttTests
{
    public class MqttHeaderTests
    {
        /// <summary>
        /// Tests decoding of a single byte payload size.
        /// </summary>
        [Fact]
        public void SingleBytePayloadSize()
        {
            // validates a payload size of a single byte using the example values supplied in the MQTT spec
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(127);

            // test that the count of bytes returned is only 1, and the value of the byte is correct.
            Assert.Equal<int>(returnedBytes.Count, 1);
            Assert.Equal<byte>(127, returnedBytes[0]);
        }

        /// <summary>
        /// Tests decoding of the lower boundary (128) of a double byte payload size
        /// </summary>
        [Fact]
        public void DoubleBytePayloadSizeLowerBoundary()
        {
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(128);

            Assert.Equal<int>(returnedBytes.Count, 2);
            Assert.Equal<byte>(0x80, returnedBytes[0]);
            Assert.Equal<byte>(0x01, returnedBytes[1]);
        }

        /// <summary>
        /// Tests decoding of the upper boundary of a double byte payload size (16383)
        /// </summary>
        [Fact]
        public void DoubleBytePayloadSizeUpperBoundary()
        {
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(16383);

            Assert.Equal<int>(returnedBytes.Count, 2);
            Assert.Equal<byte>(0xFF, returnedBytes[0]);
            Assert.Equal<byte>(0x7F, returnedBytes[1]);
        }

        /// <summary>
        /// Tests the lower boundary of the triple byte payload size (16384)
        /// </summary>
        [Fact]
        public void TripleBytePayloadSizeLowerBoundary()
        {
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(16384);

            Assert.Equal<int>(returnedBytes.Count, 3);
            Assert.Equal<byte>(0x80, returnedBytes[0]);
            Assert.Equal<byte>(0x80, returnedBytes[1]);
            Assert.Equal<byte>(0x01, returnedBytes[2]);
        }

        /// <summary>
        /// Tests the upper boundary of the triple byte payload size (2097151)
        /// </summary>
        [Fact]
        public void TripleBytePayloadSizeUpperBoundary()
        {
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(2097151);

            Assert.Equal<int>(returnedBytes.Count, 3);
            Assert.Equal<byte>(0xFF, returnedBytes[0]);
            Assert.Equal<byte>(0xFF, returnedBytes[1]);
            Assert.Equal<byte>(0x7F, returnedBytes[2]);
        }

        /// <summary>
        /// Tests the lower boundary of the quad byte payload size (2097152)
        /// </summary>        
        [Fact]
        public void QuadrupleBytePayloadSizeLowerBoundary()
        {
            // validates a payload size of a single byte using the example values supplied in the MQTT spec
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(2097152);

            Assert.Equal<int>(returnedBytes.Count, 4);
            Assert.Equal<byte>(0x80, returnedBytes[0]);
            Assert.Equal<byte>(0x80, returnedBytes[1]);
            Assert.Equal<byte>(0x80, returnedBytes[2]);
            Assert.Equal<byte>(0x01, returnedBytes[3]);
        }

        /// <summary>
        /// Tests the upper boundary of the quad byte payload size (268435455)
        /// </summary>        
        [Fact]
        public void QuadrupleBytePayloadSizeUpperBoundary()
        {
            // validates a payload size of a single byte using the example values supplied in the MQTT spec
            List<byte> returnedBytes = CallGetRemainingBytesWithValue(268435455);

            Assert.Equal<int>(returnedBytes.Count, 4);
            Assert.Equal<byte>(0xFF, returnedBytes[0]);
            Assert.Equal<byte>(0xFF, returnedBytes[1]);
            Assert.Equal<byte>(0xFF, returnedBytes[2]);
            Assert.Equal<byte>(0x7F, returnedBytes[3]);
        }

        /// <summary>
        /// Test helper method to call Get Remaining Bytes with a specific value
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private List<byte> CallGetRemainingBytesWithValue(int value)
        {
            // validates a payload size of a single byte using the example values supplied in the MQTT spec
            MqttHeader header = new MqttHeader();
            header.MessageSize = value;

            MethodInfo mi = typeof(MqttHeader).GetMethod("GetRemainingLengthBytes", ReflectionBindingConstants.NonpublicMethod);
            return (List<byte>)mi.Invoke(header, null);
        }

        /// <summary>
        /// Tests that payload sizes outside of the maximum allowed upper range are caught and exception thrown.
        /// </summary>
        [Fact]
        public void PayloadSizeOutOfUpperRange()
        {
            MqttHeader header = new MqttHeader();
            Assert.Throws<InvalidPayloadSizeException>(() => header.MessageSize = 268435456);
        }

        /// <summary>
        /// Tests that payload sizes outside of the maximum allowed lower range are caught and exception thrown.
        /// </summary>
        [Fact]
        public void PayloadSizeOutOfLowerRange()
        {
            MqttHeader header = new MqttHeader();
            Assert.Throws<InvalidPayloadSizeException>(() => header.MessageSize = -1);
        }

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
                inputHeader.WriteTo(stream);

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
        public void Deserialization_Header_CorruptHeader_InvalidMessageSize()
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
                inputHeader.WriteTo(stream);

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

        [Fact]
        public void Deserialization_Header_Qos_AtMostOnce()
        {
            var headerBytes = GetHeaderBytes(1, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.AtMostOnce, header.Qos);
        }

        [Fact]
        public void Deserialization_Header_Qos_AtLeastOnce()
        {
            var headerBytes = GetHeaderBytes(2, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, header.Qos);
        }

        [Fact]
        public void Deserialization_Header_Qos_ExactlyOnce()
        {
            var headerBytes = GetHeaderBytes(4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.ExactlyOnce, header.Qos);
        }

        [Fact]
        public void Deserialization_Header_Qos_Reserved1()
        {
            var headerBytes = GetHeaderBytes(6, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttQos>(MqttQos.Reserved1, header.Qos);
        }


        [Fact]
        public void Deserialization_Header_MessageType_Reserved1()
        {
            var headerBytes = GetHeaderBytes(0, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Reserved1, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_Connect()
        {
            var headerBytes = GetHeaderBytes(1 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Connect, header.MessageType);
        }


        [Fact]
        public void Deserialization_Header_MessageType_ConnectAck()
        {
            var headerBytes = GetHeaderBytes(2 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.ConnectAck, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_Publish()
        {
            var headerBytes = GetHeaderBytes(3 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Publish, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_PublishAck()
        {
            var headerBytes = GetHeaderBytes(4 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishAck, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_PublishReceived()
        {
            var headerBytes = GetHeaderBytes(5 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishReceived, header.MessageType);
        }


        [Fact]
        public void Deserialization_Header_MessageType_PublishRelease()
        {
            var headerBytes = GetHeaderBytes(6 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishRelease, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_PublishComplete()
        {
            var headerBytes = GetHeaderBytes(7 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PublishComplete, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_Subscribe()
        {
            var headerBytes = GetHeaderBytes(8 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Subscribe, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_SubscriptionAck()
        {
            var headerBytes = GetHeaderBytes(9 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.SubscriptionAck, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_Unsubscribe()
        {
            var headerBytes = GetHeaderBytes(10 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Unsubscribe, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_UnsubsriptionAck()
        {
            var headerBytes = GetHeaderBytes(11 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.UnsubsriptionAck, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_PingRequest()
        {
            var headerBytes = GetHeaderBytes(12 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PingRequest, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_PingResponse()
        {
            var headerBytes = GetHeaderBytes(13 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.PingResponse, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_MessageType_Disconnect()
        {
            var headerBytes = GetHeaderBytes(14 << 4, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.Equal<MqttMessageType>(MqttMessageType.Disconnect, header.MessageType);
        }

        [Fact]
        public void Deserialization_Header_Duplicate_True()
        {
            var headerBytes = GetHeaderBytes(8, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.True(header.Duplicate);
        }

        [Fact]
        public void Deserialization_Header_Duplicate_False()
        {
            var headerBytes = GetHeaderBytes(0, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.False(header.Duplicate);
        }

        [Fact]
        public void Deserialization_Header_Retain_True()
        {
            var headerBytes = GetHeaderBytes(1, 0);
            MqttHeader header = GetMqttHeader(headerBytes);
            Assert.True(header.Retain);
        }

        [Fact]
        public void Deserialization_Header_Retain_False()
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
