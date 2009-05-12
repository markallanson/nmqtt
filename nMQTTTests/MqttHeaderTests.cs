using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using nMqtt;
using System.Reflection;

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
            header.PayloadSize = value;

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
            Assert.Throws<InvalidPayloadSizeException>(() => header.PayloadSize = 268435456);
        }

        /// <summary>
        /// Tests that payload sizes outside of the maximum allowed lower range are caught and exception thrown.
        /// </summary>
        [Fact]
        public void PayloadSizeOutOfLowerRange()
        {
            MqttHeader header = new MqttHeader();
            Assert.Throws<InvalidPayloadSizeException>(() => header.PayloadSize = -1);
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
                PayloadSize = 1,
                MessageType = MqttMessageType.Connect,
                Qos = MqttQos.AtLeastOnce
            };

            PropertyInfo piGetter = typeof(MqttHeader).GetProperty("HeaderBytes", ReflectionBindingConstants.NonPublicGetter);
            List<byte> headerBytes = (List<byte>)piGetter.GetValue(inputHeader, null);

            MethodInfo myCreater = typeof(MqttHeader).GetMethod("Create", ReflectionBindingConstants.NonPublisStaticMethod, null, new [] { typeof(IEnumerable<byte>) }, null);
            MqttHeader outputHeader = (MqttHeader)myCreater.Invoke(null, new[] { (object)headerBytes });

            Assert.Equal<bool>(inputHeader.Duplicate, outputHeader.Duplicate);
            Assert.Equal<bool>(inputHeader.Retain, outputHeader.Retain);
            Assert.Equal<MqttQos>(inputHeader.Qos, outputHeader.Qos);
            Assert.Equal<MqttMessageType>(inputHeader.MessageType, outputHeader.MessageType);
        }

        /// <summary>
        /// Ensures a header with an invalid message size portion is caught and thrown correctly.
        /// </summary>
        [Fact]
        public void CorruptHeaderDeserialize_InvalidMessageSize()
        {
            MqttHeader inputHeader = new MqttHeader()
            {
                Duplicate = true,
                Retain = false,
                PayloadSize = 268435455, // the max message size, which we will fudge later in the test
                MessageType = MqttMessageType.Connect,
                Qos = MqttQos.AtLeastOnce
            };

            PropertyInfo piGetter = typeof(MqttHeader).GetProperty("HeaderBytes", ReflectionBindingConstants.NonPublicGetter);
            List<byte> headerBytes = (List<byte>)piGetter.GetValue(inputHeader, null);

            // fudge the header by making the last bit of the 4th message size byte a 1, therefore making the header
            // invalid because the last bit of the 4th size byte should always be 0 (according to the spec). It's how 
            // we know to stop processing the header when reading a full message).
            headerBytes[4] = (byte)(headerBytes[4] | 0xFF);

            MethodInfo myCreater = typeof(MqttHeader).GetMethod("Create", ReflectionBindingConstants.NonPublisStaticMethod, null, new[] { typeof(IEnumerable<byte>) }, null);
            Assert.Throws<InvalidHeaderException>(() =>
                {
                    try
                    {
                        myCreater.Invoke(null, new[] { (object)headerBytes });
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }

                });
        }
    }
}
