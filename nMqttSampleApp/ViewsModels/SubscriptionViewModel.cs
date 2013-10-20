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
	public class SubscriptionViewModel : ViewModel<SubscriptionModel>
	{
		public BindingList<string> Topics  
		{
			get
			{
				return Model.Topics;
			}
			set
			{
				Model.Topics = value;
			}
		}
		
		public string Topic
		{
			get
			{
				return Model.Topic;
			}
			set
			{
				Model.Topic = value;
			}
		}
		
		public decimal Qos
		{
			get
			{
				return (decimal)Model.Qos;
			}
			set
			{
				Model.Qos = (byte)value;
			}
		}		
		
		public decimal QosMinimum  
		{
			get
			{
				return 0;
			}
		}
		
		public decimal QosMaximum
		{
			get
			{
				return 2;
			}
		}
		
		public string MessageHistory
		{
			get
			{
				return Model.MessageHistory;
			}
		}

        public string ReceivedTopic
        {
            get
            {
                return Model.ReceivedTopic;
            }
        }
	
		public SubscriptionViewModel()
		{
			Model = new SubscriptionModel();
		}
		
		public void Subscribe()
		{
			Model.Subscribe();
		}
	}
}
