using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace nMqtt
{
    /// <summary>
    /// Implementation of the variable header for an MQTT Connect message.
    /// </summary>
    public class MqttConnectVariableHeader : MqttVariableHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        public MqttConnectVariableHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttConnectVariableHeader(Stream headerStream)
            : base(headerStream)
        {
        }

        /// <summary>
        /// Writes the variable header for an MQTT Connect message to the supplied stream.
        /// </summary>
        /// <param name="headerStream"></param>
        public override void WriteTo(System.IO.Stream headerStream)
        {
            WriteProtocolName(headerStream);
            WriteProtocolVersion(headerStream);
            WriteConnectFlags(headerStream);
            WriteKeepAlive(headerStream);
        }

        /// <summary>
        /// Creates a variable header from the specified header stream.
        /// </summary>
        /// <param name="headerStream">The header stream.</param>
        public override void ReadFrom(System.IO.Stream headerStream)
        {
            ReadProtocolName(headerStream);
            ReadProtocolVersion(headerStream);
            ReadConnectFlags(headerStream);
            ReadKeepAlive(headerStream);
        }


        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return
                String.Format("Connect Variable Header: ProtocolName={0}, ProtocolVersion={1}, ConnectFlags=({2}), KeepAlive={3}",
                    ProtocolName, ProtocolVersion, ConnectFlags, KeepAlive);
        }
    }
}
