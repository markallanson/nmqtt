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
    ///     Implementation of an MQTT rawTopic and all the validation that goes on inside it.
    /// </summary>
    internal class SubscriptionTopic : Topic
    {
        /// <summary>
        ///     Creates a new instance of a rawTopic from a topic string.
        /// </summary>
        /// <param name="RawTopic">The rawTopic to represent.</param>
        public SubscriptionTopic(string RawTopic)
            : base(RawTopic, ValidateMinLength, ValidateMaxLength, ValidateMultiWildcard, ValidateFragments) {
        }

        /// <summary>
        ///     Validates all unqiue fragments in the topic match the MQTT spec requirements.
        /// </summary>
        // ReSharper disable NotResolvedInText
        private static void ValidateFragments(Topic topicInstance) {
            // if any fragment contains a wildcard or a multi wildcard but is greater than 
            // 1 character long, then it's an error - wildcards must appear by themselves.
            var invalidFragment = topicInstance.TopicFragments.FirstOrDefault(
                fragment => (fragment.Contains(MultiWildcard) || fragment.Contains(Wildcard)) && fragment.Length > 1);
            if (invalidFragment != null) {
                throw new ArgumentException(
                    String.Format("rawTopic Fragment '{0}' contains a wildcard but is more than one character long.",  invalidFragment),
                    "topic");
            }
        }

        /// <summary>
        ///     Validates the placement of the multi-wildcard character in subscription topics.
        /// </summary>
        /// <param name="topicInstance">The instance to check.</param>
        private static void ValidateMultiWildcard(Topic topicInstance) {
            if (topicInstance.RawTopic.Contains(MultiWildcard) && !topicInstance.RawTopic.EndsWith(MultiWildcard)) {
                throw new ArgumentException("The rawTopic wildcard # can only be present at the end of a topic", "topic");
            }
            if (topicInstance.RawTopic.Length > 1 && topicInstance.RawTopic.EndsWith(MultiWildcard) && 
                !topicInstance.RawTopic.EndsWith(MultiWildcardValidEnd)) {
                throw new ArgumentException(
                    "Topics using the # wildcard longer than 1 character must be immediately preceeded by a the rawTopic separator /.", "topic");
            }
        }
        // ReSharper restore NotResolvedInText

        /// <summary>
        ///     Checks if the rawTopic matches the supplied rawTopic using the MQTT rawTopic matching rules.
        /// </summary>
        /// <param name="matcheeTopic">The rawTopic to match.</param>
        /// <returns>True if the rawTopic matches based on the MQTT rawTopic matching rules, otherwise false.</returns>
        public bool Matches(PublicationTopic matcheeTopic) {
            // of the left rawTopic is just a multi wildcard then we have a match without
            // needing to check any further.
            if (this.RawTopic.Equals(MultiWildcard)) {
                return true;
            }
            // if the topics are an exact match, bail early with a cheap comparison
            if (Equals(matcheeTopic)) {
                return true;
            }
            // no match yet so we need to check each fragment
            for (var i = 0; i < this.TopicFragments.Length; i++) {
                var lhsFragment = TopicFragments[i];
                // if we've reached a multi wildcard in the lhs rawTopic, 
                // we have a match. 
                // (this is the mqtt spec rule finance matches finance or finance/#)
                if (lhsFragment.Equals(MultiWildcard)) {
                    return true;
                }
                var isLhsWildcard = lhsFragment.Equals(Wildcard);
                // if we've reached a wildcard match but the matchee does not have anything at
                // this fragment level then it's not a match. 
                // (this is the mqtt spec rule finance does not match finance/+
                if (isLhsWildcard && matcheeTopic.TopicFragments.Length <= i) {
                    return false;
                }
                // if lhs is not a wildcard we need to check whether the
                // two fragments match each other.
                if (!isLhsWildcard) {
                    var rhsFragment = matcheeTopic.TopicFragments[i];
                    // if lhs fragment is not wildcard then we need an exact match 
                    if (!lhsFragment.Equals(rhsFragment)) {
                        return false;
                    }
                }
                // if we're at the last fragment of the lhs rawTopic but there are
                // more fragments in the in the matchee then the matchee rawTopic
                // is too specific to be a match.
                if (i + 1 == this.TopicFragments.Length &&
                    matcheeTopic.TopicFragments.Length > this.TopicFragments.Length) {
                    return false;
                }
                // if we're here the current fragment matches so check the next
            }
            // if we exit out of the loop without a return then we have a full match rawTopic/rawTopic which would
            // have been caught by the original exact match check at the top anyway.
            return true;
        }
    }
}
