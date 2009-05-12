using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    /// <summary>
    /// Exception thrown when processing a header that is invalid in some way.
    /// </summary>
    public class InvalidHeaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidHeaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidHeaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
