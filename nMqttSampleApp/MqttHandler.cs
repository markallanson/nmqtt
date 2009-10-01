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
using Nmqtt;

namespace nMqtt.SampleApp
{
	/// <summary>
	/// Singleton for consuming mqtt functionality.
	/// </summary>
	public class MqttHandler
	{
		private static MqttHandler instance = new MqttHandler ();

		private MqttClient client;

		private MqttHandler ()
		{
		}

		public static MqttHandler Instance 
		{
			get { return instance; }
		}

		public ConnectionState Connect(string server, short port)
		{
			System.Windows.Forms.MessageBox.Show("About to connect");
			client = new MqttClient(server, port, Options.ClientIdentifier);
			return client.Connect();
		}
		
		public void Disconnect()
		{
			client.Dispose();
		}
	}
}
