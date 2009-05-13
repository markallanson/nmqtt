using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    /// <summary>
    /// Exception thrown when a client identifier included in a message is too long.
    /// </summary>
    public class ClientIdentifierException : Exception
    {
        private const string MessageTemplate = "The client identifier {0} is too long ({1}). Maximum ClientIdentifier length is {2}";

        public string ClientIdentifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentifierException"/> class.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        public ClientIdentifierException(string clientIdentifier)
            : base(String.Format(MessageTemplate, clientIdentifier, clientIdentifier.Length, Constants.MaxClientIdentifierLength))
        {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentifierException"/> class.
        /// </summary>
        /// <param name="length">The length of the client identifier.</param>
        /// <param name="clientIdentifier">The client identifier.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientIdentifierException(string clientIdentifier, Exception innerException)
            : base(String.Format(MessageTemplate, clientIdentifier, clientIdentifier.Length, Constants.MaxClientIdentifierLength), innerException)
        {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentifierException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ClientIdentifierException(string clientIdentifier, string message)
            : base(message)
        {
            ClientIdentifier = clientIdentifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentifierException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ClientIdentifierException(string clientIdentifier, string message, Exception innerException)
            : base(message, innerException)
        {
            ClientIdentifier = clientIdentifier;
        }
    }
}
