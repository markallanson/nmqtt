using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    public class InvalidPayloadSizeException : Exception
    {
        private const string MessageConstant = "The size of the payload ({0} bytes) must be equal to or greater than 0 and less than {1} bytes)";

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPayloadSizeException"/> class.
        /// </summary>
        /// <param name="payloadSize">Size of the payload.</param>
        public InvalidPayloadSizeException(int payloadSize, int maxSize)
            : base(String.Format(MessageConstant, payloadSize, maxSize))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPayloadSizeException"/> class.
        /// </summary>
        /// <param name="payloadSize">Size of the payload.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidPayloadSizeException(int payloadSize, int maxSize, Exception innerException)
            : base(String.Format(MessageConstant, payloadSize, maxSize), innerException)
        {
        }
    }
}
