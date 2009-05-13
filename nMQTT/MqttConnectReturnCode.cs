using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    /// <summary>
    /// Enumeration of allowable connection request return codes.
    /// </summary>
    public enum MqttConnectReturnCode : byte
    {
        ConnectionAccepted = 0,
        UnacceptedProtocolVersion,
        IdentifierRejected,
        BrokerUnavailable
    }
}
