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
    ///     Enumeration of allowable connection request return codes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "Clarity with MQTT Spec")]
    internal enum MqttConnectReturnCode : byte
    {
        ConnectionAccepted = 0,
        UnacceptedProtocolVersion,
        IdentifierRejected,
        BrokerUnavailable,
        BadUsernameOrPassword,
        NotAuthorized
    }
}
