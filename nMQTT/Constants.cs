using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nMqtt
{
    internal class Constants
    {
        /// <summary>
        /// The Maximum allowed message size as defined by the MQTT v3 Spec (256MB).
        /// </summary>
        public const int MaxMessageSize = 268435455;
    }
}
