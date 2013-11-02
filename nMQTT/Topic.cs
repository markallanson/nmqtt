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
using System.Linq;

namespace Nmqtt 
{
    /// <summary>
    /// Imlplementation of an MQTT topic and all the validation that goes on inside it.
    /// </summary>
    internal class Topic
    {
        private const string      TopicSeparator        = "/";
        private const string      MultiWildcard         = "#";
        private const string      MultiWildcardValidEnd = TopicSeparator + MultiWildcard;
        private const string      Wildcard              = "+";
        private const int         MaxTopicLength        = 65535;

        private readonly string   topic;
        private readonly string[] topicFragments;

        /// <summary>
        /// Returns True if there are any wildcards in the specified topic, otherwise false.
        /// </summary>
        public Boolean HasWildcards {
            get { return this.topic.Contains(MultiWildcard) || this.topic.Contains(Wildcard); }
        }

        /// <summary>
        /// Creates a new instance of a topic from a topic string.
        /// </summary>
        /// <param name="topic">The topic to represent.</param>
        public Topic(string topic) {
            this.topic = topic;
            this.topicFragments = topic.Split(TopicSeparator[0]);
            ValidateTopic();
        }

        /// <summary>
        /// Validates the topic meets the MQTT spec.
        /// </summary>
        // ReSharper disable NotResolvedInText
        private void ValidateTopic() {
            if (String.IsNullOrWhiteSpace(topic)) {
                throw new ArgumentException("Topic must contain at least one character.", "topic");
            }
            if (topic.Length > MaxTopicLength) {
                throw new ArgumentException(
                    String.Format("The length of the supplied topic ({0}) is longer than the maximum allowable ({1})", topic.Length, MaxTopicLength), 
                    "topic");                
            }
            if (topic.Contains(MultiWildcard) && !topic.EndsWith(MultiWildcard)) {
                throw new ArgumentException("The topic wildcard # can only be present at the end of a topic", "topic");
            }
            if (topic.Length > 1 && topic.EndsWith(MultiWildcard) && !topic.EndsWith(MultiWildcardValidEnd)) {
                throw new ArgumentException(
                    "Topics using the # wildcard longer than 1 character must be immediately preceeded by a the topic separator /.", "topic");
            }
            // if any fragment contains a wildcard or a multi wildcard but is greater than 
            // 1 character long, then it's an error - wildcards must appear by themselves.
            var invalidFragment = this.topicFragments.FirstOrDefault(
                fragment => (fragment.Contains(MultiWildcard) || fragment.Contains(Wildcard)) && fragment.Length > 1);
            if (invalidFragment != null) {
                throw new ArgumentException(
                    String.Format("Topic Fragment '{0}' contains a wildcard but is more than one character long.", invalidFragment),
                    "topic");
            }
        }
        // ReSharper restore NotResolvedInText

        /// <summary>
        /// Checks if the topic matches the supplied topic using the MQTT topic matching rules.
        /// </summary>
        /// <param name="matcheeTopic">The topic to match.</param>
        /// <returns>True if the topic matches based on the MQTT topic matching rules, otherwise false.</returns>
        public bool Matches(String matcheeTopic) {
            return Matches(new Topic(matcheeTopic));
        }

        /// <summary>
        /// Checks if the topic matches the supplied topic using the MQTT topic matching rules.
        /// </summary>
        /// <param name="matcheeTopic">The topic to match.</param>
        /// <returns>True if the topic matches based on the MQTT topic matching rules, otherwise false.</returns>
        public bool Matches(Topic matcheeTopic) {
            // of the left topic is just a multi wildcard then we have a match without
            // needing to check any further.
            if (this.topic.Equals(MultiWildcard)) {
                return true;
            }
            // if the topics are an exact match, bail early with a cheap comparison
            if (Equals(matcheeTopic)) {
                return true;
            }
            // no match yet so we need to check each fragment
            for (var i = 0; i < this.topicFragments.Length; i++) {
                var lhsFragment = topicFragments[i];
                // if we've reached a multi wildcard in the lhs topic, 
                // we have a match. 
                // (this is the mqtt spec rule finance matches finance or finance/#)
                if (lhsFragment.Equals(MultiWildcard)) {
                    return true;
                }
                var isLhsWildcard = lhsFragment.Equals(Wildcard);
                // if we've reached a wildcard match but the matchee does not have anything at
                // this fragment level then it's not a match. 
                // (this is the mqtt spec rule finance does not match finance/+
                if (isLhsWildcard && matcheeTopic.topicFragments.Length <= i) {
                    return false;
                }
                // if lhs is not a wildcard we need to check whether the
                // two fragments match each other.
                if (!isLhsWildcard) {
                    var rhsFragment = matcheeTopic.topicFragments[i];
                    // if lhs fragment is not wildcard then we need an exact match 
                    if (!lhsFragment.Equals(rhsFragment)) {
                        return false;
                    }
                }
                // if we're at the last fragment of the lhs topic but there are
                // more fragments in the in the matchee then the matchee topic
                // is too specific to be a match.
                if (i + 1 == this.topicFragments.Length &&
                    matcheeTopic.topicFragments.Length > this.topicFragments.Length) {
                    return false;
                }
                // if we're here the current fragment matches so check the next
            }
            // if we exit out of the loop without a return then we have a full match topic/topic which would
            // have been caught by the original exact match check at the top anyway.
            return true;
        }

        /// <summary>
        /// Serves as a hash function for a topics.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:Nmqtt.Topic"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() {
            return this.topic.GetHashCode();
        }

        /// <summary>
        /// Checks if one topic equals another topic exactly. 
        /// </summary>
        /// <param name="obj">The topic to compare equality to.</param>
        public override bool Equals(object obj) {
            if (obj is String) {
                return this.topic.Equals(obj);
            }
            var rhsTopic = obj as Topic;
            return rhsTopic != null && this.topic.Equals(rhsTopic.ToString());
        }

        /// <summary>
        /// Returns a String representation of the topic.
        /// </summary>
        /// <returns>A string representation of the topic.</returns>
        public override string ToString() {
            return this.topic;
        }
    }
}
