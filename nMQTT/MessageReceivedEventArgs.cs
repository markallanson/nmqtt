/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Event arguments for the Message Received event.
    /// </summary>
    internal class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the Message that has been received.
        /// </summary>
        /// <value>The Message stream.</value>
        public MqttMessage Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="MessageStream">The Message that has been received.</param>
        public MessageReceivedEventArgs(MqttMessage message)
        {
            this.Message = message;
        }
    }
}
