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
using System.Threading;

namespace nMqtt.SampleApp
{
	/// <summary>
	/// Singleton for consuming mqtt functionality.
	/// </summary>
	public class MqttHandler
	{
		private static MqttHandler instance = new MqttHandler ();

        /// <summary>
        /// The instance of the underlying MqttClient that is connected to the server.
        /// </summary>
		private MqttClient client;

        /// <summary>
        /// Synchronization context that the mqtthandler uses to invoke the message arrived events on the same thread that connected.
        /// </summary>
        private SynchronizationContext syncContext;

		private MqttHandler ()
		{
		}

        /// <summary>
        /// The instance of the MqttHandler that manages to the Mqtt connection.
        /// </summary>
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
            syncContext = SynchronizationContext.Current;

			Trace.WriteLine ("Connecting to " + server + ":" + port.ToString ());
			
			return client.Connect(Options.Username, Options.Password);
		}

		private void ClientMessageAvailable (object sender, MqttMessageEventArgs e)
		{
			OnClientMessageArrived(e);
		}

        /// <summary>
        /// Disconnects from the Mqtt server.
        /// </summary>
		public void Disconnect()
		{
			client.Dispose();
		}

        /// <summary>
        /// Subscribes to the specified topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="qos">The qos.</param>
		public void Subscribe(string topic, byte qos)
		{
            if (client == null) throw new InvalidOperationException("You must connect before you can subscribe to a topic.");

			client.Subscribe(topic, (MqttQos)qos);

            Trace.WriteLine(String.Format("Subscribed to Topic '{0}'.", topic));
            if (TopicSubscribed != null)
            {
                syncContext.Post((data) => this.TopicSubscribed(instance, new TopicSubscribedEventArgs(topic)), null);
            }
		}

        /// <summary>
        /// Publish message to the specified topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="qos">The qos.</param>
        /// <param name="data">The message.</param>
        public void Publish(string topic, byte qos, object data)
        {
            if (client == null) throw new InvalidOperationException("You must connect before you can subscribe to a topic.");

            client.PublishMessage<AsciiPublishDataConverter>(topic, (MqttQos)qos, data);
        }

        /// <summary>
        /// Event fired when subscribed to a new topic
        /// </summary>
        /// TODO: MqttTopicSubscribedEventArgs
        public event EventHandler<TopicSubscribedEventArgs> TopicSubscribed;

        /// <summary>
		/// Event fired when a message arrives from the remote server.
		/// </summary>
		public event EventHandler<MqttMessageEventArgs> ClientMessageArrived;
		private void OnClientMessageArrived (MqttMessageEventArgs e)
		{
			Trace.WriteLine (String.Format ("Message Arrived on Topic '{0}'.", e.Topic));
			if (ClientMessageArrived != null)
			{
                syncContext.Post((data) => this.ClientMessageArrived(instance, e), null);
			}
		}
	}
}
