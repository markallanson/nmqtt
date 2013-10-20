/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("nMqtt Library")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("nMqtt")]
[assembly: AssemblyProduct("nMqtt")]
[assembly: AssemblyCopyright("Copyright © nMQTT Project 2009-2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("07ec48ef-9569-4156-8ed8-4b0f91c3cc94")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("0.1.1.0")]
[assembly: AssemblyFileVersion("0.1.1.0")]

// Let the unit test assembly see our internal bits

[assembly:
    InternalsVisibleTo(
        "NmqttTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010085148ff4e2ccb76efc8a2712a23781825f7bc93a82e531ad190cbaa56ba7ad95fdeaadb2f756fe42f4cec6e8129ad5b67880ff73bca7cb21ff7dc622b01221f11e7f993fb3891983a3b78d856ee566d7712876f6a695f3adf4f05624a9435006d0066c2ccc2bf6933b9c6c5c4c328d23a67dce8a652edeffcf40ea61202ace9d"
        )]

// and Moq and Castle Windsor

[assembly:
    InternalsVisibleTo(
        "Moq, PublicKey=00240000048000009400000006020000002400005253413100040000010001009f7a95086500f8f66d892174803850fed9c22225c2ccfff21f39c8af8abfa5415b1664efd0d8e0a6f7f2513b1c11659bd84723dc7900c3d481b833a73a2bcf1ed94c16c4be64d54352c86956c89930444e9ac15124d3693e3f029818e8410f167399d6b995324b635e95353ba97bfab856abbaeb9b40c9b160070c6325e22ddc"
        )]
[assembly:
    InternalsVisibleTo(
        "DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7"
        )]