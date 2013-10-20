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

namespace NmqttTests.MessageIdentifierDispenser
{
    public class MessageIdentifierDispatcherTests
    {
        [Fact]
        public void NewSequenceStartsNumberingAtOne() {
            var dispenser = new Nmqtt.MessageIdentifierDispenser();
            Assert.Equal<int>(1, dispenser.GetNextMessageIdentifier(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void SequenceIncrementsByOneForEachCall()
        {
            var dispenser = new Nmqtt.MessageIdentifierDispenser();
            int first = dispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic");
            int second = dispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic");
            Assert.Equal<int>(first + 1, second);
        }

        [Fact]
        public void SequenceOverflowRollsBackToOne()
        {
            var dispenser = new Nmqtt.MessageIdentifierDispenser();
            for (
                int i = dispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow");
                i < short.MaxValue;
                i = dispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow"))
            {
            }

            // one more call should overflow us and reset us back to 1.
            Assert.Equal<int>(1, dispenser.GetNextMessageIdentifier("Topic::Sample/My/Topic/Overflow"));
        }
    }
}
