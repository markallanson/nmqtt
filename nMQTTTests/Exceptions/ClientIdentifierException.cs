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

namespace NmqttTests.Exceptions
{
    public class ClientIdentifierException
    {
        [Fact]
        public void DefaultCtor()
        {
            try
            {
                throw new Nmqtt.ClientIdentifierException();
            }
            catch (Nmqtt.ClientIdentifierException ex)
            {
                // ensure we got a message with the default ctor.
                Assert.True(ex.Message.Length > 0);
            }
        }

        [Fact]
        public void Ctor_ClientIdentifier()
        {
            try
            {
                throw new Nmqtt.ClientIdentifierException("abc");
            }
            catch (Nmqtt.ClientIdentifierException ex)
            {
                // ensure we got a message with the default ctor.
                Assert.Equal<string>("abc", ex.ClientIdentifier);
            }
        }

        [Fact]
        public void Ctor_ClientIdentifierAndException()
        {
            try
            {
                throw new Nmqtt.ClientIdentifierException("abc", new ArgumentException("arg"));
            }
            catch (Nmqtt.ClientIdentifierException ex)
            {
                // ensure we got a message with the default ctor.
                Assert.Equal<string>("abc", ex.ClientIdentifier);
                Assert.NotNull(ex.InnerException);
            }
        }

        [Fact]
        public void Ctor_ClientIdentifierAndMessage()
        {
            try
            {
                throw new Nmqtt.ClientIdentifierException("abc", "hello");
            }
            catch (Nmqtt.ClientIdentifierException ex)
            {
                // ensure we got a message with the default ctor.
                Assert.Equal<string>("abc", ex.ClientIdentifier);
                Assert.Equal<string>("hello", ex.Message);
            }
        }

        [Fact]
        public void Ctor_ClientIdentifierAndMessageAndException()
        {
            try
            {
                throw new Nmqtt.ClientIdentifierException("abc", "hello", new ArgumentException("arg"));
            }
            catch (Nmqtt.ClientIdentifierException ex)
            {
                // ensure we got a message with the default ctor.
                Assert.Equal<string>("abc", ex.ClientIdentifier);
                Assert.Equal<string>("hello", ex.Message);
                Assert.NotNull(ex.InnerException);
            }
        }
    }
}
