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
    ///     Event arguments that represent the data present within an invalid message.
    /// </summary>
    public class InvalidMessageEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets or sets the Exception that describes the invalid message problem.
        /// </summary>
        /// <value>The data stream.</value>
        public InvalidMessageException Exception { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidMessageEventArgs" /> class.
        /// </summary>
        /// <param name="exception">The Exception that describes the message problem.</param>
        public InvalidMessageEventArgs(InvalidMessageException exception) {
            this.Exception = exception;
        }
    }
}