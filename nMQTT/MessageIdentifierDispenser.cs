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

using System.Collections.Generic;

namespace Nmqtt
{
    internal class MessageIdentifierDispenser
    {
        private readonly Dictionary<string, short> idStorage = new Dictionary<string, short>();

        /// <summary>
        ///     Used to synchronise access
        /// </summary>
        private static readonly object idPadlock = new object();

        /// <summary>
        ///     Gets the next message identifier for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public short GetNextMessageIdentifier(string key) {
            // only a single id can be dispensed at a time, regardless of the key. 
            // Will revise to per-key locking if it proves bottleneck
            lock (idPadlock) {
                if (!idStorage.ContainsKey(key)) {
                    idStorage.Add(key, 1); // add a new key, start at 1, 0 is reserved for by MQTT spec for invalid msg.
                    return 1;
                } else {
                    short nextId = ++idStorage[key];
                    if (nextId == short.MinValue) {
                        // overflow, wrap back to 1.
                        nextId = idStorage[key] = 1;
                    }
                    return nextId;
                }
            }
        }
    }
}