using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    /// Enumeration of allowable connection request return codes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification="Clarity with MQTT Spec")]
    public enum MqttConnectReturnCode : byte
    {
        ConnectionAccepted = 0,
        UnacceptedProtocolVersion,
        IdentifierRejected,
        BrokerUnavailable
    }
}
