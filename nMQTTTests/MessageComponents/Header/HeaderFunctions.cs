/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
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
using Xunit;
using Nmqtt;
using System.Reflection;
using System.IO;

namespace NmqttTests.MessageComponents.Header
{
    public class HeaderFunctions
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
    }
}
