using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Nmqtt;

namespace NmqttTests.DataConverters
{
    public class AsciiStringDataConverterTests
    {
        [Fact]
        public void AsciiStringToByteArray()
        {
            var testString = "testStringA-Z,1-9,a-z";
            var conv = new AsciiPublishDataConverter();
            byte[] bytes = conv.ConvertToBytes(testString);

            Assert.Equal<int>(testString.Length, bytes.Length);
            for (int i = 0; i < testString.Length; i++)
            {
                Assert.Equal<byte>((byte)testString[i], bytes[i]);
            }
        }

        [Fact]
        public void ByteArrayToAsciiString()
        {
            var input = new byte[] { 40, 41, 42, 43 };
            var conv = new AsciiPublishDataConverter();
            var output = (string)conv.ConvertFromBytes(input);

            Assert.Equal<int>(input.Length, output.Length);
            for (int i = 0; i < input.Length; i++)
            {
                Assert.Equal<byte>(input[i], (byte)output[i]);
            }
        }
    }
}
