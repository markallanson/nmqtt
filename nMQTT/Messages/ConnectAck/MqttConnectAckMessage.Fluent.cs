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
    ///     Message that indicates a connection acknowledgement.
    /// </summary>
    internal sealed partial class MqttConnectAckMessage
    {
        /// <summary>
        ///     Sets the return code of the Variable Header.
        /// </summary>
        /// <param name="returnCode">The return code to set.</param>
        /// <returns>The new MqttConnectAckMessage with the return code set.</returns>
        public MqttConnectAckMessage WithReturnCode(MqttConnectReturnCode returnCode) {
            this.VariableHeader.ReturnCode = returnCode;
            return this;
        }
    }
}