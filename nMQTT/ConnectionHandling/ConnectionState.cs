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

namespace Nmqtt
{
    /// <summary>
    ///     Enumeration that indicates various client connection states
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        ///     The MQTT Connection is in the process of disconnecting from the broker.
        /// </summary>
        Disconnecting,

        /// <summary>
        ///     The MQTT Connection is not currently connected to any broker.
        /// </summary>
        Disconnected,

        /// <summary>
        ///     The MQTT Connection is in the process of connecting to the broker.
        /// </summary>
        Connecting,

        /// <summary>
        ///     The MQTT Connection is currently connected to the broker.
        /// </summary>
        Connected,

        /// <summary>
        ///     The MQTT Connection is faulted and no longer communicating with the broker.
        /// </summary>
        Faulted
    }
}