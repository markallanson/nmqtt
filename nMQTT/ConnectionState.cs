using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nmqtt
{
    /// <summary>
    /// Enumeration that indicates various client connection states
    /// </summary>
    public enum ConnectionState
    {
        Disconnected,
        Connected,
        Idle
    }
}
