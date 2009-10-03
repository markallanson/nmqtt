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

        /// <summary>
        /// Connects to the specified mqtt server.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <returns></returns>
		public ConnectionState Connect(string server, short port)
		{
			client = new MqttClient(server, port, Options.ClientIdentifier);
			return client.Connect();
		}
		
		public void Disconnect()
		{
			client.Dispose();
		}
		
		public void Subscribe(string topic, byte qos)
		{
			client.Subscribe(topic, (MqttQos)qos, (subTopic, data) => true);
		}
	}
}
