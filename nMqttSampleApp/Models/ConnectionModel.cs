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
using System.ComponentModel;

namespace nMqtt.SampleApp.Models
{
	public class ConnectionModel : Model, IConnectionModel
	{
		private const short DefaultPort = 1883;
	
		public ConnectionModel ()
		{
			Servers = new BindingList<string> ();
			Ports = new BindingList<short> ();
			Ports.Add (DefaultPort);

			Port = DefaultPort;
		}
		
		public BindingList<short> Ports { get; set; }
		
		public BindingList<string> Servers { get; set; }
		
		public string Server { get; set; }
		
		public short Port { get; set; }

		public void Connect ()
		{
			if (!Servers.Contains (Server)) Servers.Add (Server);
			if (!Ports.Contains (Port)) Ports.Add(Port);

            MqttHandler.Instance.Connect(Server, Port);
		}
		
		public void Disconnect()
		{
            MqttHandler.Instance.Disconnect();
		}
	}
}
