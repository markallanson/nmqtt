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
	public interface ISubscriptionModel : IModel
	{
		BindingList<string> Topics
		{
			get;
			set;
		}
		
		string Topic
		{
			get;
			set;
		}
		
		byte Qos
		{
			get;
			set;
		}
		
		string MessageHistory
		{
			get;
			set;
		}
		
		void Subscribe();
	}
}
