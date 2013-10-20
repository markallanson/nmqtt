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
using System.Collections.ObjectModel;

namespace Nmqtt.ExtensionMethods
{
    /// <summary>
    ///     Helper methods to provide functionality on Collection[T].
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        ///     Provides AddRange functionality to Collection[T]
        /// </summary>
        /// <param name="collection">The collection to add a range of values to.</param>
        /// <param name="range">The range of valus to add to the collection.</param>
        public static void AddRange(this Collection<byte> collection, IEnumerable<byte> range) {
            foreach (var b in range) {
                collection.Add(b);
            }
        }

        /// <summary>
        ///     Provides AddRange functionality to Collection[T]
        /// </summary>
        /// <param name="collection">The collection to add a range of values to.</param>
        /// <param name="range">The range of valus to add to the collection.</param>
        public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> range) {
            foreach (var item in range) {
                collection.Add(item);
            }
        }
    }
}