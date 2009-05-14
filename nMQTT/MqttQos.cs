using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    /// Enumeration of available QoS types.
    /// </summary>
    public enum MqttQos
    {
        AtMostOnce = 0,
        AtLeastOnce,
        ExactlyOnce,
        Reserved1
    }
}
