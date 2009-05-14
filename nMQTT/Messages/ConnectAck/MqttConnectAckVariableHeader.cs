using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Implementation of the variable header for an MQTT ConnectAck message.
    /// </summary>
    public sealed class MqttConnectAckVariableHeader : MqttVariableHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        public MqttConnectAckVariableHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectVariableHeader"/> class.
        /// </summary>
        /// <param name="headerStream">A stream containing the header of the message.</param>
        public MqttConnectAckVariableHeader(Stream headerStream)
            : base(headerStream)
        {
        }

        /// <summary>
        /// Writes the variable header for an MQTT Connect message to the supplied stream.
        /// </summary>
        /// <param name="headerStream"></param>
        public override void WriteTo(System.IO.Stream headerStream)
        {
            // TODO: Replace this once "Topic Compression" byte that is in connectack spec but not variableheader spec has been resolved.           
            headerStream.WriteByte(0);

            WriteReturnCode(headerStream);
        }

        /// <summary>
        /// Creates a variable header from the specified header stream.
        /// </summary>
        /// <param name="headerStream">The header stream.</param>
        public override void ReadFrom(System.IO.Stream headerStream)
        {
            // TODO: Replace this once "Topic Compression" byte that is in connectack spec but not variableheader spec has been resolved.           
            headerStream.ReadByte();
 
            ReadReturnCode(headerStream);
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
                String.Format("Connect Variable Header: TopicNameCompressionResponse={0}, ReturnCode={1}",
                    0, ReturnCode);
        }
    }
}
