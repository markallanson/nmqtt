/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2010 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.ComponentModel;
using System.Text;

namespace nMqtt.SampleApp
{
	public class SubscriptionModel : Model, ISubscriptionModel
	{
		private bool subscribedToEvent;
	
		public BindingList<string> Topics  
		{
			get;
			set;
		}
		
		public string Topic
		{
			get;
			set;
		}
		
		public byte Qos
		{
			get;
			set;
		}
		
        string receivedTopic;
        public string ReceivedTopic
        {
            get { return receivedTopic; }
            set
            {
                receivedTopic = value;
                OnPropertyChanged("ReceivedTopic");
            }
        }

		string messageHistory;
		public string MessageHistory
		{
			get { return messageHistory; }
			set 
			{ 
				messageHistory = value; 
				OnPropertyChanged("MessageHistory");
			}
		}
		
		public SubscriptionModel()
		{
			Topics = new BindingList<string>();
		}
		
		public void Subscribe ()
		{
			if (!Topics.Contains (Topic))
				Topics.Add (Topic);

			if (!subscribedToEvent)
			{
				subscribedToEvent = true;
				MqttHandler.Instance.ClientMessageArrived += HandleMqttHandlerInstanceClientMessageArrived;
			}
			
			MqttHandler.Instance.Subscribe (Topic, Qos);
			MessageHistory += Environment.NewLine + "Subscribed to: " + Topic;
			
		}

        public void Unsubscribe() {
            MqttHandler.Instance.Unsubscribe(Topic);
        }

		void HandleMqttHandlerInstanceClientMessageArrived (object sender, Nmqtt.MqttMessageEventArgs e)
		{
            // Assume the arrived message is a byte array that contains a simple ASCII string.
            var messagePublished = Encoding.ASCII.GetString((byte[])e.Message);
			MessageHistory += String.Format("{0}: {1}{2}", e.Topic, messagePublished, Environment.NewLine);	
		}

	}
}
