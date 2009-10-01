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

namespace NmqttTests.EventArgs
{
    public class ConnectionDroppedEventArgsTests
    {
        [Fact]
        public void CtorSetsProperty()
        {
            string testMsg = "test msg";

            ConnectionDroppedEventArgs e = new ConnectionDroppedEventArgs(new Exception(testMsg));
            Assert.NotNull(e.Exception);
            Assert.Equal<string>(testMsg, e.Exception.Message);
        }
    }
}
