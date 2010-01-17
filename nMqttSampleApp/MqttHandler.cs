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
using System.Diagnostics;

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

        /// <summary>
        /// Connects to the specified mqtt server.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns>The state of the connection.</returns>
		public ConnectionState Connect (string server, short port)
		{
			client = new MqttClient (server, port, Options.ClientIdentifier);
			client.MessageAvailable += ClientMessageAvailable;
			
			Trace.WriteLine ("Connecting to " + server + ":" + port.ToString ());
			
			return client.Connect();
		}

		private void ClientMessageAvailable (object sender, MqttMessageEventArgs e)
		{
			OnClientMessageArrived(e);
		}
		
		public void Disconnect()
		{
			client.Dispose();
		}
		
		public void Subscribe(string topic, byte qos)
		{
			client.Subscribe(topic, (MqttQos)qos);
		}
		
		/// <summary>
		/// Event fired when a message arrives from the remote server.
		/// </summary>
		public event EventHandler<MqttMessageEventArgs> ClientMessageArrived;
		private void OnClientMessageArrived (MqttMessageEventArgs e)
		{
			Trace.WriteLine (String.Format ("Message Arrived on Topic '{0}'.", e.Topic));
			if (ClientMessageArrived != null)
			{
				this.ClientMessageArrived(instance, e);
			}
		}
	}
}
