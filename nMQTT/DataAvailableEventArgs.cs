/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.ObjectModel;

namespace Nmqtt
{
    /// <summary>
    ///     Event arguments for the Data Available event fired by the MqttConnection class.
    /// </summary>
    public class DataAvailableEventArgs : EventArgs
    {
        private readonly Collection<byte> messageData;

        /// <summary>
        ///     Gets or sets the data stream that contains the data to read from.
        /// </summary>
        /// <value>The data stream.</value>
        public Collection<byte> MessageData {
            get { return messageData; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataAvailableEventArgs" /> class.
        /// </summary>
        /// <param name="messageData">A collection of bytes containing the message data.</param>
        public DataAvailableEventArgs(Collection<byte> messageData) {
            this.messageData = messageData;
        }
    }
}