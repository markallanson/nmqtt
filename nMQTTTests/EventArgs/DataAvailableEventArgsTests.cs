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
using System.Collections.ObjectModel;

namespace NmqttTests.EventArgs
{
    public class DataAvailableEventArgsTests
    {
        [Fact]
        public void CtorDataAvailableInProperty()
        {
            var e = new DataAvailableEventArgs(new List<byte>(new byte[] { 3, 2, 1 }));
            Assert.Equal<int>(3, e.MessageData.Count());
            Assert.Equal<byte>(3, e.MessageData[0]);
            Assert.Equal<byte>(2, e.MessageData[1]);
            Assert.Equal<byte>(1, e.MessageData[2]);
        }
    }
}
