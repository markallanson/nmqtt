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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;


namespace Nmqtt.ExtensionMethods
{
    /// <summary>
    /// Helper methods to provide functionality on Collection[T].
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Provides AddRange functionality to Collection[T]
        /// </summary>
        /// <param name="collection">The collection to add a range of values to.</param>
        /// <param name="range">The range of valus to add to the collection.</param>
        public static void AddRange(this Collection<byte> collection, IEnumerable<byte> range)
        {
            foreach (byte b in range)
            {
                collection.Add(b);
            }
        }
		
        /// <summary>
        /// Provides AddRange functionality to Collection[T]
        /// </summary>
        /// <param name="collection">The collection to add a range of values to.</param>
        /// <param name="range">The range of valus to add to the collection.</param>
   		public static void AddRange<T>(this Collection<T> collection, IEnumerable<T> range)
		{
            foreach (T item in range)
            {
                collection.Add(item);
            }			
		}
    }
}
