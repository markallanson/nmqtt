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
    ///     Interface that defines the methods and properties that must be provided by classes
    ///     that interpret and convert inbound and outbound published message data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Types that implement this interface should be aware that for the purposes of converting
    ///         data from published messages (byte array to object model) that the MqttSubscriptionsManager
    ///         creates a single instance of the data converter and uses it for all messages that are
    ///         received.
    ///     </para>
    ///     <para>
    ///         The same is true for the publishing of data to a broker. The PublishingManager will
    ///         also cache instances of the converters until the MqttClient is disposed.
    ///     </para>
    ///     <para>
    ///         This means, in both cases you can store state in the data converters if you wish, and
    ///         that state will persist between messages received or published, but only a default empty
    ///         constructor is supported.
    ///     </para>
    /// </remarks>
    public interface IPayloadConverter<T>
    {
        /// <summary>
        ///     Converts received data from a raw byte array to an object graph.
        /// </summary>
        /// <param name="messageData">The received data as an array of bytes.</param>
        /// <returns>The data processed and turned into the specified type.</returns>
        T ConvertFromBytes(byte[] messageData);

        /// <summary>
        ///     Converts sent data from an object graph to a byte array.
        /// </summary>
        /// <param name="data">The data to convert to the byte array.</param>
        /// <returns>A byte array representation of the data.</returns>
        byte[] ConvertToBytes(T data);
    }
}