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
using System.Security.Permissions;

namespace Nmqtt
{
    /// <summary>
    ///     Exception that is thrown when the payload of a message is not the correct size.
    /// </summary>
    [Serializable]
    public class InvalidTopicException : Exception
    {
        private readonly string topic;

        /// <summary>
        ///     The topic that is invalid
        /// </summary>
        public string Topic {
            get { return this.topic;  }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidTopicException" /> class.
        /// </summary>
        /// <param name="message">The reason the topic is invalid.</param>
        /// <param name="topic">The topic that was invalid.</param>
        public InvalidTopicException(string message, string topic)
            : base(message) {
            this.topic = topic;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidTopicException" /> class.
        /// </summary>
        /// <param name="message">The reason the topic is invalid.</param>
        /// <param name="topic">The topic that was invalid.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidTopicException(string message, string topic, Exception innerException)
            : base(message, innerException) {
                this.topic = topic;            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        ///     The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected InvalidTopicException(System.Runtime.Serialization.SerializationInfo info,
                                        System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {}

        /// <summary>
        ///     When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info,
                                           System.Runtime.Serialization.StreamingContext context) {
            if (info == null) {
                throw new System.ArgumentNullException("info");
            }

            base.GetObjectData(info, context);
        }
    }
}