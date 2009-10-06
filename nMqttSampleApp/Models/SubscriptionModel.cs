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
using System.ComponentModel;

namespace nMqtt.SampleApp
{
	public class SubscriptionModel : Model, ISubscriptionModel
	{
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
		
		public SubscriptionModel()
		{
			Topics = new BindingList<string>();
		}
		
		public void Subscribe()
		{
			if (!Topics.Contains(Topic)) Topics.Add(Topic);
			
			MqttHandler.Instance.Subscribe(Topic, Qos);
		}
	}
}
