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

namespace Nmqtt
{
    internal static class Constants
    {
        /// <summary>
        ///     The Maximum allowed message size as defined by the MQTT v3 Spec (256MB).
        /// </summary>
        public const int MaxMessageSize = 268435455;

        /// <summary>
        ///     The Maximum allowed client identifier length.
        /// </summary>
        public const int MaxClientIdentifierLength = 23;

        /// <summary>
        ///     The default Mqtt port to connect to.
        /// </summary>
        public const int DefaultMqttPort = 1883;

        /// <summary>
        ///     The recommended length for usernames and passwords.
        /// </summary>
        public const int RecommendedMaxUsernamePasswordLength = 12;
    }
}