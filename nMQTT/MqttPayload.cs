using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nMqtt
{
    /// <summary>
    /// Represents the payload (Body) of an MQTT Message.
    /// </summary>
    public class MqttPayload
    {
        /// <summary>
        /// Writes the payload to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to s the variable header to.</param>
        /// <remarks>
        /// A basic message has no Variable Header.
        /// </remarks>
        public virtual void WriteTo(Stream payloadStream) { }

        /// <summary>
        /// Creates a payload from the specified header stream.
        /// </summary>
        /// <param name="headerStream">The header stream.</param>
        public virtual void ReadFrom(Stream payloadStream) { }

    }
}
