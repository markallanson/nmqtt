/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Linq;
using System.ComponentModel;
using nMqtt.SampleApp.Models;

namespace nMqtt.SampleApp.ViewModels
{
	public class ConnectionViewModel : ViewModel<IConnectionModel>
	{
		public ConnectionViewModel ()
		{
			Model = new ConnectionModel();
		}
		
		public BindingList<short> Ports 
		{
			get
			{
				return Model.Ports;
			}
			set
			{
				Model.Ports = value;
			} 
		}
		
		public BindingList<string> Servers
		{
			get
			{
				return Model.Servers;
			}
			set
			{
				Model.Servers = value;
			} 
		}
		
		public string Server
		{
			get
			{
				return Model.Server;
			}
			set
			{
				Model.Server = value;
			} 
		}
		
		public short Port 
		{
			get
			{
				return Model.Port;
			}
			set
			{
				Model.Port = value;
			} 
		}

		public void Connect ()
		{
			Model.Connect();			
		}
		
		public void Disconnect ()
		{
			Model.Disconnect();
		}
	}
}
