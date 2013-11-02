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

using System;
using System.IO;
using Nmqtt.Encoding;
using Nmqtt.ExtensionMethods;

namespace Nmqtt
{
    /// <summary>
    ///     Represents the base class for the Variable Header portion of some MQTT Messages.
    /// </summary>
    internal class MqttVariableHeader
    {
        private int length;

        /// <summary>
        ///     The length, in bytes, consumed by the variable header.
        /// </summary>
        public int Length {
            get {
                // TODO: improve the way that we calculate the variable header length somehow
                return length;
            }
        }

        public string ProtocolName { get; set; }
        public byte ProtocolVersion { get; set; }
        public MqttConnectFlags ConnectFlags { get; set; }

        /// <summary>
        ///     Defines the maximum allowable lag, in seconds, between expected messages.
        /// </summary>
        /// <remarks>
        ///     The spec indicates that clients won't be disconnected until KeepAlive + 1/2 KeepAlive time period
        ///     elapses.
        /// </remarks>
        public short KeepAlive { get; set; }

        public MqttConnectReturnCode ReturnCode { get; set; }
        public string TopicName { get; set; }
        public short MessageIdentifier { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttVariableHeader" /> class.
        /// </summary>
        public MqttVariableHeader() {
            this.ProtocolName = "MQIsdp";
            this.ProtocolVersion = 3;
            this.ConnectFlags = new MqttConnectFlags();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttVariableHeader" /> class, populating it with data from a stream.
        /// </summary>
        /// <param name="headerStream">The stream containing the variable header.</param>
        public MqttVariableHeader(Stream headerStream) {
            ReadFrom(headerStream);
        }

        /// <summary>
        ///     Gets the Read Flags used during message deserialization
        /// </summary>
        protected virtual ReadWriteFlags ReadFlags {
            get { return 0; }
        }

        /// <summary>
        ///     Gets the write flags used during message serialization
        /// </summary>
        protected virtual ReadWriteFlags WriteFlags {
            get { return 0; }
        }

        /// <summary>
        ///     Writes the variable header to the supplied stream.
        /// </summary>
        /// <param name="variableHeaderStream">The stream to write the variable header to.</param>
        /// <remarks>
        ///     This base implementation uses the WriteFlags property that can be
        ///     overridden in subclasses to determine what to read from the variable header.
        ///     A subclass can override this method to do completely custom read operations
        ///     if required.
        /// </remarks>
        public virtual void WriteTo(Stream variableHeaderStream) {
            if ((WriteFlags & ReadWriteFlags.ProtocolName) == ReadWriteFlags.ProtocolName) {
                WriteProtocolName(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.ProtocolVersion) == ReadWriteFlags.ProtocolVersion) {
                WriteProtocolVersion(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.ConnectFlags) == ReadWriteFlags.ConnectFlags) {
                WriteConnectFlags(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.KeepAlive) == ReadWriteFlags.KeepAlive) {
                WriteKeepAlive(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.ReturnCode) == ReadWriteFlags.ReturnCode) {
                WriteReturnCode(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.TopicName) == ReadWriteFlags.TopicName) {
                WriteTopicName(variableHeaderStream);
            }
            if ((WriteFlags & ReadWriteFlags.MessageIdentifier) == ReadWriteFlags.MessageIdentifier) {
                WriteMessageIdentifier(variableHeaderStream);
            }
        }

        /// <summary>
        ///     Creates a variable header from the specified header stream.
        /// </summary>
        /// <param name="variableHeaderStream">The stream to read the variable header from.</param>
        /// <remarks>
        ///     This base implementation uses the ReadFlags property that can be
        ///     overridden in subclasses to determine what to read from the variable header.
        ///     A subclass can override this method to do completely custom read operations
        ///     if required.
        /// </remarks>
        public virtual void ReadFrom(Stream variableHeaderStream) {
            if ((ReadFlags & ReadWriteFlags.ProtocolName) == ReadWriteFlags.ProtocolName) {
                ReadProtocolName(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.ProtocolVersion) == ReadWriteFlags.ProtocolVersion) {
                ReadProtocolVersion(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.ConnectFlags) == ReadWriteFlags.ConnectFlags) {
                ReadConnectFlags(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.KeepAlive) == ReadWriteFlags.KeepAlive) {
                ReadKeepAlive(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.ReturnCode) == ReadWriteFlags.ReturnCode) {
                ReadReturnCode(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.TopicName) == ReadWriteFlags.TopicName) {
                ReadTopicName(variableHeaderStream);
            }
            if ((ReadFlags & ReadWriteFlags.MessageIdentifier) == ReadWriteFlags.MessageIdentifier) {
                ReadMessageIdentifier(variableHeaderStream);
            }
        }


        /// <summary>
        ///     Gets the length of the write data when WriteTo will be called.
        /// </summary>
        /// <returns>The length of data witten by the call to GetWriteLength</returns>
        public virtual int GetWriteLength() {
            int headerLength = 0;
            var enc = new MqttEncoding();

            if ((WriteFlags & ReadWriteFlags.ProtocolName) == ReadWriteFlags.ProtocolName) {
                headerLength += enc.GetByteCount(ProtocolName);
            }
            if ((WriteFlags & ReadWriteFlags.ProtocolVersion) == ReadWriteFlags.ProtocolVersion) {
                headerLength += sizeof (byte);
            }
            if ((WriteFlags & ReadWriteFlags.ConnectFlags) == ReadWriteFlags.ConnectFlags) {
                headerLength += MqttConnectFlags.GetWriteLength();
            }
            if ((WriteFlags & ReadWriteFlags.KeepAlive) == ReadWriteFlags.KeepAlive) {
                headerLength += sizeof (short);
            }
            if ((WriteFlags & ReadWriteFlags.ReturnCode) == ReadWriteFlags.ReturnCode) {
                headerLength += sizeof (byte);
            }
            if ((WriteFlags & ReadWriteFlags.TopicName) == ReadWriteFlags.TopicName) {
                headerLength += enc.GetByteCount(TopicName.ToString());
            }
            if ((WriteFlags & ReadWriteFlags.MessageIdentifier) == ReadWriteFlags.MessageIdentifier) {
                headerLength += sizeof (short);
            }

            return headerLength;
        }


        protected void WriteProtocolName(Stream stream) {
            stream.WriteMqttString(ProtocolName);
        }

        protected void WriteProtocolVersion(Stream stream) {
            stream.WriteByte(ProtocolVersion);
        }

        protected void WriteKeepAlive(Stream stream) {
            stream.WriteShort(KeepAlive);
        }

        protected void WriteReturnCode(Stream stream) {
            stream.WriteByte((byte) ReturnCode);
        }

        protected void WriteTopicName(Stream stream) {
            stream.WriteMqttString(TopicName.ToString());
        }

        protected void WriteMessageIdentifier(Stream stream) {
            stream.WriteShort(MessageIdentifier);
        }

        protected void WriteConnectFlags(Stream stream) {
            ConnectFlags.WriteTo(stream);
        }

        protected void ReadProtocolName(Stream stream) {
            ProtocolName = stream.ReadMqttString();
            length += ProtocolName.Length + 2; // 2 for length short at front of string
        }

        protected void ReadProtocolVersion(Stream stream) {
            ProtocolVersion = (byte) stream.ReadByte();
            length++;
        }

        protected void ReadKeepAlive(Stream stream) {
            KeepAlive = stream.ReadShort();
            length += 2;
        }

        protected void ReadReturnCode(Stream stream) {
            ReturnCode = (MqttConnectReturnCode) stream.ReadByte();
            length++;
        }

        protected void ReadTopicName(Stream stream) {
            TopicName = stream.ReadMqttString();
            length += TopicName.Length + 2; // 2 for length short at front of string.
        }

        protected void ReadMessageIdentifier(Stream stream) {
            MessageIdentifier = stream.ReadShort();
            length += 2;
        }

        protected void ReadConnectFlags(Stream stream) {
            ConnectFlags = new MqttConnectFlags(stream);
            length += 1;
        }

        /// <summary>
        ///     Enumeration used by subclasses to tell the variable header what should be read from the underlying stream.
        /// </summary>
        [Flags]
        protected enum ReadWriteFlags
        {
            ProtocolName = 1,
            ProtocolVersion = 2,
            ConnectFlags = 4,
            KeepAlive = 8,
            ReturnCode = 16,
            TopicName = 32,
            MessageIdentifier = 64
        }
    }
}