using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using nMqtt.ExtensionMethods;

namespace nMqtt
{
    /// <summary>
    /// Represents the base class for the Variable Header portion of some MQTT Messages.
    /// </summary>
    public class MqttVariableHeader
    {
        public string ProtocolName { get; set; }
        public byte ProtocolVersion { get; set; }
        public MqttConnectFlags ConnectFlags { get; set; }

        /// <summary>
        /// Defines the maximum allowable lag, in seconds, between expected messages.
        /// </summary>
        /// <remarks>
        /// The spec indicates that clients won't be disconnected until KeepAlive + 1/2 KeepAlive time period
        /// elapses.
        /// </remarks>
        public short KeepAlive { get; set; }

        public MqttConnectReturnCode ReturnCode { get; set; }
        public string TopicName { get; set; }
        public short MessageIdentifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttVariableHeader"/> class.
        /// </summary>
        public MqttVariableHeader()
        {
            this.ProtocolName = "MQIspd";
            this.ProtocolVersion = 3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttVariableHeader"/> class, populating it with data from a stream.
        /// </summary>
        /// <param name="headerStream">The stream containing the variable header.</param>
        public MqttVariableHeader(Stream headerStream)
        {
            ReadFrom(headerStream);
        }

        /// <summary>
        /// Writes the variable header to the supplied stream.
        /// </summary>
        /// <param name="messageStream">The stream to s the variable header to.</param>
        /// <remarks>
        /// A basic message has no Variable Header.
        /// </remarks>
        public virtual void WriteTo(Stream headerStream) { }

        /// <summary>
        /// Creates a variable header from the specified header stream.
        /// </summary>
        /// <param name="headerStream">The header stream.</param>
        public virtual void ReadFrom(Stream headerStream) { }

        protected void WriteProtocolName(Stream stream)
        {
            stream.WriteMqttString(ProtocolName);
        }

        protected void WriteProtocolVersion(Stream stream)
        {
            stream.WriteByte(ProtocolVersion);
        }

        protected void WriteKeepAlive(Stream stream)
        {
            stream.WriteShort(KeepAlive);
        }

        protected void WriteReturnCode(Stream stream)
        {
            stream.WriteByte((byte)ReturnCode);
        }

        protected void WriteTopicName(Stream stream)
        {
            stream.WriteMqttString(TopicName);
        }

        protected void WriteMessageIdentifier(Stream stream)
        {
            stream.WriteShort(MessageIdentifier);
        }

        protected void WriteConnectFlags(Stream stream)
        {
            ConnectFlags.WriteTo(stream);
        }

        protected void ReadProtocolName(Stream stream)
        {
            ProtocolName = stream.ReadMqttString();
        }

        protected void ReadProtocolVersion(Stream stream)
        {
            ProtocolVersion = (byte)stream.ReadByte();
        }

        protected void ReadKeepAlive(Stream stream)
        {
            KeepAlive = stream.ReadShort();
        }

        protected void ReadReturnCode(Stream stream)
        {
            ReturnCode = (MqttConnectReturnCode)stream.ReadByte();
        }

        protected void ReadTopicName(Stream stream)
        {
            TopicName = stream.ReadMqttString();           
        }

        protected void ReadMessageIdentifier(Stream stream)
        {
            MessageIdentifier = stream.ReadShort();
        }

        protected void ReadConnectFlags(Stream stream)
        {
            ConnectFlags = new MqttConnectFlags(stream);
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
                String.Format("Variable Header: ProtocolName={0}, ProtocolVersion={1}, ConnectFlags=({2}), KeepAlive={3}, " +
                    "ReturnCode={4}, TopicName={5}, MessageIdentfier={6}",
                    ProtocolName, ProtocolVersion, ConnectFlags, KeepAlive, ReturnCode, TopicName, MessageIdentifier);
        }
    }
}
