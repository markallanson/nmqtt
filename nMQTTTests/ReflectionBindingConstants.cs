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
using System.Reflection;

namespace NmqttTests
{
    public class ReflectionBindingConstants
    {
        public const BindingFlags NonpublicMethod = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod;
        public const BindingFlags NonPublicGetter = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty;
        public const BindingFlags NonPublicSetter = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
        public const BindingFlags PublicStaticMethod = BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static;
        public const BindingFlags NonPublicStaticMethod = BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static;
        public const BindingFlags NonPublicConstructor = BindingFlags.NonPublic | BindingFlags.CreateInstance;
        public const BindingFlags PublicConstructor = BindingFlags.Public | BindingFlags.CreateInstance;
        public const BindingFlags PublicMethod = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
    }
}
