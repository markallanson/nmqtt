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
    public class ConnectionException : Exception
    {
        private const string MessageTemplate =
            "The connection must be in the Connected state in order to perform this operation. Current state is {0}";

        /// <summary>
        ///     The connection state that caused the exception.
        /// </summary>
        public ConnectionState ConnectionState { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="connectionState">State of the connection.</param>
        public ConnectionException(ConnectionState connectionState)
            : base(String.Format(MessageTemplate, connectionState)) {
            ConnectionState = connectionState;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="connectionState">State of the connection.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConnectionException(ConnectionState connectionState, Exception innerException)
            : base(String.Format(MessageTemplate, connectionState), innerException) {
            ConnectionState = connectionState;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConnectionException(string message)
            : base(message) {
            ConnectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception that caused the connection problem</param>
        public ConnectionException(string message, Exception innerException)
            : base(message, innerException) {
            ConnectionState = ConnectionState.Disconnected;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="connectionState">The current connection state.</param>
        public ConnectionException(string message, ConnectionState connectionState)
            : this(message) {
            ConnectionState = connectionState;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientIdentifierException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="connectionState">The current connection state.</param>
        public ConnectionException(string message, ConnectionState connectionState, Exception innerException)
            : this(message, innerException) {
            ConnectionState = connectionState;
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
        protected ConnectionException(System.Runtime.Serialization.SerializationInfo info,
                                      System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
            if (info == null) {
                throw new ArgumentNullException("info");
            }

            ConnectionState = (ConnectionState) info.GetUInt32("ConnectionState");
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
                throw new System.ArgumentNullException("info");
            }

            base.GetObjectData(info, context);

            info.AddValue("ConnectionState", ConnectionState);
        }
    }
}