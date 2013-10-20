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

namespace Nmqtt
{
    /// <summary>
    ///     Event Arguments that represent a connection dropped reason.
    /// </summary>
    public class ConnectionDroppedEventArgs : EventArgs
    {
        /// <summary>
        ///     The exception that caused the connection to drop.
        /// </summary>
        /// <value>The </value>
        public Exception Exception { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectionDroppedEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The Exception that describes the reason for disconnection.</param>
        public ConnectionDroppedEventArgs(Exception exception) {
            this.Exception = exception;
        }
    }
}