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

using Nmqtt.ExtensionMethods;
using NmqttTests;
using System.IO;
using System.Reflection;

namespace nMqttTests.ExtensionMethods
{
    public class StreamExtensionMethods
    {
        /// <summary>
        /// Ensures we get the correct exception when we say there's more bytes than there actually are.
        /// </summary>
        [Fact(Skip="Reading strings changed to use repeatable reads against the buffer to support slow network connections therefore no implementation of 'too few characters' at the moment.")]
        public void String_NotEnoughBytesInString()
        {
            Type emType = GetStreamExtensionMethod();

            var bytes = new[] { (byte)0, (byte)2, (byte)'m' };
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    emType.InvokeMember("ReadMqttString", ReflectionBindingConstants.PublicStaticMethod, null, null, new object[] { ms });
                }
                catch (TargetInvocationException ex)
                {
                    Assert.IsType<ArgumentException>(ex.InnerException);
                }
            }
        }

        /// <summary>
        /// Ensures we get the correct exception when we say there's more bytes than there actually are.
        /// </summary>
        [Fact]
        public void String_LongStringIsFullyRead() {
            var emType = GetStreamExtensionMethod();

            var longString = new byte[65535];  // Longest possible 65k string full of character 'a'
            for (var i = 0; i < longString.Length; i++) {
                longString[i] = (byte)'a';
            }
            var expectedString = System.Text.Encoding.ASCII.GetString(longString);
            var bytes = new List<byte>(new[]{ (byte)(longString.Length >> 8), (byte)(longString.Length & 0xFF) });
            bytes.AddRange(longString);
            
            using (var ms = new MemoryStream(bytes.ToArray())) { 
                var result = (String) emType.InvokeMember("ReadMqttString", ReflectionBindingConstants.PublicStaticMethod, null, null, new object[] { ms });
                Assert.Equal(expectedString, result);
            }
        }
        /// <summary>
        /// Ensures we get an exception when we don't have enough bytes to describe a string.
        /// </summary>
        [Fact]
        public void String_NotEnoughBytesToFormString()
        {
            Type emType = GetStreamExtensionMethod();

            var bytes = new[] { (byte)0 };
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                try
                {
                    emType.InvokeMember("ReadMqttString", ReflectionBindingConstants.PublicStaticMethod, null, null, new object[] { ms });
                }
                catch (TargetInvocationException ex)
                {
                    Assert.IsType<ArgumentException>(ex.InnerException);
                }
            }
        }

        public Type GetStreamExtensionMethod()
        {
            return Type.GetType("Nmqtt.ExtensionMethods.StreamExtensions, Nmqtt");
        }
    }
}
