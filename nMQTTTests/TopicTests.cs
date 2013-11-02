/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2013 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/
using System;
using Nmqtt;
using Xunit;

namespace NmqttTests {
    public class TopicTests 
    {
        /*
         * Tests for topic validation and matching
         */
        [Fact]
        public void InvalidMultiWildcardAtEndOfTopicThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("invalidEnding#"));
        }

        [Fact]
        public void MultiWildcardInMiddleOfTopicThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("a/#/topic"));
        }        
        
        [Fact]
        public void TopicWithMoreThanOneMultiWildcardInSingleFragmentThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("a/##/topic"));
        }        
        
        [Fact]
        public void TopicWithMoreThanOneTypeOfWildcardInSingleFragmentThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("a/#+/topic"));
        }

        [Fact]
        public void TopicWithMoreThanOneWildcardInSingleFragmentThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("a/++/topic"));
        }

        [Fact]
        public void TopicWithMoreThanJustWildcardInFragmentThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Topic("a/frag+/topic"));
        }
        
        [Fact]
        public void TopicWithLengthLongerThanMaxAllowedThrowsArgumentException() {
            var longTopic = new char[65536];
            for (int i = 0; i < longTopic.Length; i++) {
                longTopic[i] = 'a';
            }
            Assert.Throws<ArgumentException>(() => new Topic(new string(longTopic)));
        }

        [Fact]
        public void MultiWildcardAtEndOfTopicIsValidWhenPreceededByTopicSeparator() {
            new Topic("a/topic/#");
        }

        [Fact]
        public void TopicWithNoWildcardsOfAnyTypeIsValid() {
            new Topic("a/topic/with/no/wildcard/is/good");
        }

        [Fact]
        public void TopicWithNoSeparatorsIsValid() {
            new Topic("ATopicWithNoSeparators");
        }

        /*
         * Tests for topic matching - valid matches
         */
        [Fact]
        public void SingleLevelEqualTopicsMatch() {
            var topic = new Topic("finance");
            Assert.True(topic.Matches("finance"));
        }
        
        [Fact]
        public void MultiWildcardOnlyTopicMatchesAnyRandomTopic() {
            var topic = new Topic("#");
            Assert.True(topic.Matches("finance/ibm/closingprice"));
        }

        [Fact]
        public void MultiWildcardOnlyTopicMatchesTopicStartingWithSeparator() {
            var topic = new Topic("#");
            Assert.True(topic.Matches("/finance/ibm/closingprice"));
        }

        [Fact]
        public void TopicWithMultiWildcardAtEndMatchesTopicThatDoesNotMatchSameDepth() {
            var topic = new Topic("finance/#");
            Assert.True(topic.Matches("finance"));
        }

        [Fact]
        public void TopicWithMultiWildcardAtEndMatchesTopicWithAnythingAtWildcardLevel() {
            var topic = new Topic("finance/#");
            Assert.True(topic.Matches("finance/ibm"));
        }

        [Fact]
        public void TopicWithSingleWildcardAtEndMatchesAnythingInSameLevel() {
            var topic = new Topic("finance/+/closingprice");
            Assert.True(topic.Matches("finance/ibm/closingprice"));
        }

        [Fact]
        public void TopicWithMoreThanOneSingleWildcardAtDifferentLevelsMatchesTopicWithAnyValueAtThoseLevels() {
            var topic = new Topic("finance/+/closingprice/month/+");
            Assert.True(topic.Matches("finance/ibm/closingprice/month/october"));
        }

        [Fact]
        public void TopicWithSingleAndMultiWildcardMatchesTopicWithAnyValueAtThoseLevelsAndDeeper() {
            var topic = new Topic("finance/+/closingprice/month/#");
            Assert.True(topic.Matches("finance/ibm/closingprice/month/october/2014"));
        }

        /*
         * Tests for cases where topics should not match
         */
        [Fact]
        public void SingleLevelNonEqualTopicsDoNotMatch() {
            var topic = new Topic("finance");
            Assert.False(topic.Matches("money"));
        }

        [Fact]
        public void TopicWithSingleWildcardAtEndDoesNotMatchTopicThatGoesDeeper() {
            var topic = new Topic("finance/+");
            Assert.False(topic.Matches("finance/ibm/closingprice"));
        }

        [Fact]
        public void TopicWithSingleWildcardAtEndDoesNotMatchTopicThatDoesNotContainAnythingAtSameLevel() {
            var topic = new Topic("finance/+");
            Assert.False(topic.Matches("finance"));
        }        
        
        [Fact]
        public void MultiLevelNonEqualTopicsDoNotMatch() {
            var topic = new Topic("finance/ibm/closingprice");
            Assert.False(topic.Matches("some/random/topic"));
        }

        [Fact]
        public void TopicWithMultiWildcardDoesNotMatchTopicWithDifferenceBeforeWildcardLevel() {
            var topic = new Topic("finance/#");
            Assert.False(topic.Matches("money/ibm"));
        }       

        /*
         * Misc other tests
         */
        [Fact]
        public void ToStringReturnsSameTopicAsInput() {
            const string expectedTopicString = "finance/ibm";
            var topic = new Topic(expectedTopicString);
            Assert.Equal(expectedTopicString, topic.ToString());
        }
        
        [Fact]
        public void TopicWithWildcardReportsItHasWildcards() {
            Assert.True(new Topic("finance/+").HasWildcards);
        }

        [Fact]
        public void TopicWithMultiWildcardReportsItHasWildcards() {
            Assert.True(new Topic("finance/#").HasWildcards);
        }

        [Fact]
        public void TopicWithoutWildcardsReportsItDoesNotHaveWildcards() {
            Assert.False(new Topic("finance/ibm").HasWildcards);
        }
    }
}
