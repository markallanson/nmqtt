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
    /// <summary>
    ///     Enumeration of available QoS types.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification = "Consistency with MQTT Spec")]
    public enum MqttQos : byte
    {
        /// <summary>
        ///     QOS Level 0 - Message is not guaranteed delivery. No retries are made to ensure delivery is successful.
        /// </summary>
        AtMostOnce = 0,

        /// <summary>
        ///     QOS Level 1 - Message is guaranteed delivery. It will be delivered at least one time, but may be delivered
        ///     more than once if network errors occur.
        /// </summary>
        AtLeastOnce,

        /// <summary>
        ///     QOS Level 2 - Message will be delivered once, and only once. Message will be retried until
        ///     it is successfully sent..
        /// </summary>
        ExactlyOnce,

        /// <summary>
        ///     Reserved by the MQTT Spec. Currently unused.
        /// </summary>
        Reserved1
    }
}