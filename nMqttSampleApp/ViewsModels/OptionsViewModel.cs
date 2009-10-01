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

namespace nMqtt.SampleApp
{
	public class OptionsViewModel : ViewModel<IOptionsModel>
	{
		public OptionsViewModel()
		{
			Model = new OptionsModel();
		}
	
		public string ClientIdentifier 
		{
			get
			{
				return Model.ClientIdentifier;
			}
			set
			{
				Model.ClientIdentifier = value;
			}
		}
	}
}
