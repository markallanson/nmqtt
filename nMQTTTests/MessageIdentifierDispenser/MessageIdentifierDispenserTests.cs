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

namespace NmqttTests.MessageIdentifierDispenser
{
    public class MessageIdentifierDispatcherTests
    {
        [Fact]
        public void NewSequenceStartsNumberingAtOne()
        {
            Assert.Equal<int>(1, Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void SequenceIncrementsByOneForEachCall()
        {
            int first = Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic");
            int second = Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic");
            Assert.Equal<int>(first + 1, second);
        }

        [Fact]
        public void SequenceOverflowRollsBackToOne()
        {
            for (
                int i = Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow");
                i < short.MaxValue;
                i = Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow"))
            {
            }

            // one more call should overflow us and reset us back to 1.
            Assert.Equal<int>(1, Nmqtt.MessageIdentifierDispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow"));
        }
    }
}
