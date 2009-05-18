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

namespace Nmqtt
{
    internal static class MessageIdentifierDispenser
    {
        private static Dictionary<string, short> idStorage = new Dictionary<string, short>();

        /// <summary>
        /// Used to synchronise access
        /// </summary>
        private static object idPadlock = new object();

        /// <summary>
        /// Gets the next message identifier for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static short GetNextMessageIdentifier(string key)
        {
            // only a single id can be dispensed at a time, regardless of the key. 
            // Will revise to per-key locking if it proves bottleneck
            lock (idPadlock)
            {
                if (!idStorage.ContainsKey(key))
                {
                    idStorage.Add(key, 0); // add a new key
                    return 0;
                }
                else
                {
                    return ++idStorage[key];
                }
            }
        }
    }
}
