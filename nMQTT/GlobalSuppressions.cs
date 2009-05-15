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

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag", Scope = "member", Target = "Nmqtt.MqttConnectFlags.#WillFlag")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Nmqtt.MqttMessage.#.ctor(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Nmqtt.MqttMessage.#.ctor(Nmqtt.MqttHeader)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved", Scope = "member", Target = "Nmqtt.MqttMessageType.#Reserved1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved", Scope = "member", Target = "Nmqtt.MqttMessageType.#Reserved2")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Nmqtt.MqttPayload.#.ctor(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#.ctor(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Scope = "type", Target = "Nmqtt.MqttQos")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#ConnectFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#ReadConnectFlags(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#ReadFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#WriteConnectFlags(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#WriteFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "type", Target = "Nmqtt.MqttVariableHeader+ReadWriteFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttVariableHeader+ReadWriteFlags.#ConnectFlags")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1700:DoNotNameEnumValuesReserved", Scope = "member", Target = "Nmqtt.MqttQos.#Reserved1")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag", Scope = "member", Target = "Nmqtt.MqttConnectPayload.#.ctor(System.IO.Stream,System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttConnectFlags.#WriteTo(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Scope = "member", Target = "Nmqtt.MqttConnectFlags.#.ctor(System.IO.Stream)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Nmqtt.MqttVariableHeader.#GetWriteLength()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames", MessageId = "messageSize", Scope = "member", Target = "Nmqtt.MqttHeader.#WriteTo(System.Int32,System.IO.Stream)")]
