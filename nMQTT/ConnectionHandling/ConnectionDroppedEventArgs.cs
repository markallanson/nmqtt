using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    /// Event Arguments that represent a connection dropped reason. 
    /// </summary>
    public class ConnectionDroppedEventArgs : EventArgs
    {
        /// <summary>
        /// The exception that caused the connection to drop.
        /// </summary>
        /// <value>The </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionDroppedEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The Exception that describes the reason for disconnection.</param>
        public ConnectionDroppedEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
