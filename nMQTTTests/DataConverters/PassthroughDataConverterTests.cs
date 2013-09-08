using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;
using Nmqtt;

namespace NmqttTests.DataConverters
{
    public class PassthroughDataConverterTests
    {
        [Fact]
        public void PassthroughToByteArray()
        {
            var input = new byte[] { 40, 41, 42, 43 }; 
            var conv = new Nmqtt.PassThroughPayloadConverter();
            byte[] output = conv.ConvertToBytes(input);

            Assert.Equal<int>(input.Length, output.Length);
            for (int i = 0; i < input.Length; i++)
            {
                Assert.Equal<byte>((byte)input[i], output[i]);
            }
        }

        [Fact]
        public void ByteArrayToPassthrough()
        {
            var input = new byte[] { 40, 41, 42, 43 };
            var conv = new PassThroughPayloadConverter();
            var output = (byte[])conv.ConvertFromBytes(input);

            Assert.Equal<int>(input.Length, output.Length);
            for (int i = 0; i < input.Length; i++)
            {
                Assert.Equal<byte>(input[i], (byte)output[i]);
            }
        }
    }
}
