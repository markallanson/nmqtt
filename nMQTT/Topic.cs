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

namespace Nmqtt 
{
    /// <summary>
    ///     Provides the base implementation of an MQTT topic.
    /// </summary>
    /// <remarks>
    ///     An MQTT 
    /// </remarks>
    internal abstract class Topic
    {
        private const string       TopicSeparator = "/";
        protected const string     MultiWildcard = "#";
        protected const string     MultiWildcardValidEnd = TopicSeparator + MultiWildcard;
        protected const string     Wildcard = "+";
        protected const int        MaxTopicLength = 65535;

        private readonly string   rawTopic;
        private readonly string[] topicFragments;

        public string              RawTopic       { get { return this.rawTopic; } }
        public string[]            TopicFragments { get { return this.topicFragments; } }

        /// <summary>
        ///     Returns True if there are any wildcards in the specified rawTopic, otherwise false.
        /// </summary>
        public Boolean HasWildcards {
            get { return this.rawTopic.Contains(MultiWildcard) || this.rawTopic.Contains(Wildcard); }
        }

        /// <summary>
        ///     Creates a new instance of a rawTopic from a rawTopic string.
        /// </summary>
        /// <param name="rawTopic">The topic to represent.</param>
        /// <param name="validations">The validations to run on the rawTopic.</param>
        public Topic(string rawTopic, params Action<Topic>[] validations) {
            this.rawTopic = rawTopic;
            this.topicFragments = RawTopic.Split(TopicSeparator[0]);
            
            // run all validations
            foreach (var validation in validations) {
                validation(this);
            }
        }

        // ReSharper disable NotResolvedInText
        /// <summary>
        ///     Validates that the topic does not exceed the maximum length.
        /// </summary>
        /// <param name="topicInstance">The instance to check.</param>
        protected static void ValidateMaxLength(Topic topicInstance) {
            if (topicInstance.RawTopic.Length > MaxTopicLength) {
                throw new ArgumentException(
                    String.Format("The length of the supplied rawTopic ({0}) is longer than the maximum allowable ({1})",
                                  topicInstance.RawTopic.Length, MaxTopicLength),
                    "topic");
            }
        }

        /// <summary>
        ///     Validates that the topic does not fall below the minimum length.
        /// </summary>
        /// <param name="topicInstance">The instance to check.</param>
        protected static void ValidateMinLength(Topic topicInstance) {
            if (String.IsNullOrWhiteSpace(topicInstance.RawTopic)) {
                throw new ArgumentException("rawTopic must contain at least one character.", "topic");
            }
        }
        // ReSharper restore NotResolvedInText


        /// <summary>
        ///     Serves as a hash function for a topics.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:Nmqtt.Topic"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode() {
            return this.rawTopic.GetHashCode();
        }

        /// <summary>
        ///     Checks if one topic equals another topic exactly. 
        /// </summary>
        /// <param name="obj">The topic to compare equality to.</param>
        public override bool Equals(object obj) {
            if (obj is String) {
                return this.rawTopic.Equals(obj);
            }
            var rhsTopic = obj as Topic;
            return rhsTopic != null && this.rawTopic.Equals(rhsTopic.ToString());
        }

        /// <summary>
        ///     Returns a String representation of the topic.
        /// </summary>
        /// <returns>A string representation of the topic.</returns>
        public override string ToString() {
            return this.rawTopic;
        }
    }
}
