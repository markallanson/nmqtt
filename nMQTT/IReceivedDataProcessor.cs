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

namespace Nmqtt
{
    /// <summary>
    /// Interface that defines the methods and properties that must be provided by classes
    /// that interpret and process inbound published message data.
    /// </summary>
    public interface IReceivedDataProcessor
    {
        /// <summary>
        /// Processes received data and returns it as a strongly typed object.
        /// </summary>
        /// <param name="messageData">The received data as an array of bytes.</param>
        /// <returns>The data processed and turned into the specified type.</returns>
        object Process(byte[] messageData);
    }
}
