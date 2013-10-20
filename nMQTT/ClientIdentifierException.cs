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
using System.Security.Permissions;

namespace Nmqtt
{
    /// <summary>
    ///     Exception thrown when a client identifier included in a message is too long.
    /// </summary>
    [Serializable]
    public class ClientIdentifierException : Exception
    {
        private const string MessageTemplate =
            "The client identifier {0} is too long ({1}). Maximum ClientIdentifier length is {2}";

        /// <summary>
        ///     The client identifier that was incorrect.
        /// </summary>
        public string ClientIdentifier { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="clientIdentifier">The client identifier.</param>
        public ClientIdentifierException(string clientIdentifier)
            : base(
                String.Format(MessageTemplate, clientIdentifier, clientIdentifier.Length,
                              Constants.MaxClientIdentifierLength)) {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientIdentifierException(string clientIdentifier, Exception innerException)
            : base(
                String.Format(MessageTemplate, clientIdentifier, clientIdentifier.Length,
                              Constants.MaxClientIdentifierLength), innerException) {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="message">The message.</param>
        public ClientIdentifierException(string clientIdentifier, string message)
            : base(message) {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="clientIdentifier">The client idenfitier that caused the problem.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientIdentifierException(string clientIdentifier, string message, Exception innerException)
            : base(message, innerException) {
            ClientIdentifier = clientIdentifier;
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
        protected ClientIdentifierException(System.Runtime.Serialization.SerializationInfo info,
                                            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
            if (info == null) {
                throw new ArgumentNullException("info");
            }

            ClientIdentifier = info.GetString("ClientIdentifier");
        }

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
                throw new ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("ClientIdentifier", ClientIdentifier);
        }
    }
}