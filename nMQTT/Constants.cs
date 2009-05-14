/* 
 * nMQTT, a .Net MQTT v3 client implementation.
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
    internal static class Constants
    {
        /// <summary>
        /// The Maximum allowed message size as defined by the MQTT v3 Spec (256MB).
        /// </summary>
        public const int MaxMessageSize = 268435455;

        /// <summary>
        /// The Maximum allowed client identifier length.
        /// </summary>
        public const int MaxClientIdentifierLength = 23;
    }
}
