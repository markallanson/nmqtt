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
    ///     Exception that is thrown when the payload of a message is not the correct size.
    /// </summary>
    [Serializable]
    public class InvalidPayloadSizeException : Exception
    {
        private const string MessageConstant =
            "The size of the payload ({0} bytes) must be equal to or greater than 0 and less than {1} bytes)";

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidPayloadSizeException" /> class.
        /// </summary>
        /// <param name="payloadSize">Size of the payload.</param>
        /// <param name="maxSize">The maximum allowed size of the payload.</param>
        public InvalidPayloadSizeException(int payloadSize, int maxSize)
            : base(String.Format(MessageConstant, payloadSize, maxSize)) {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidPayloadSizeException" /> class.
        /// </summary>
        /// <param name="payloadSize">Size of the payload.</param>
        /// <param name="maxSize">The maximum allowable size of the payload.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidPayloadSizeException(int payloadSize, int maxSize, Exception innerException)
            : base(String.Format(MessageConstant, payloadSize, maxSize), innerException) {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidPayloadSizeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidPayloadSizeException(string message)
            : base(message) {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidPayloadSizeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidPayloadSizeException(string message, Exception innerException)
            : base(message, innerException) {}

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
        protected InvalidPayloadSizeException(System.Runtime.Serialization.SerializationInfo info,
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