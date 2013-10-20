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
	public class OptionsModel : Model, IOptionsModel
	{
		public OptionsModel ()
		{
		}
		
		public string ClientIdentifier {
			get
			{
				return Options.ClientIdentifier;
			}
			set
			{
				Options.ClientIdentifier = value;
			}
		}

        public string Username
        {
            get
            {
                return Options.Username;
            }
            set
            {
                Options.Username = value;
            }
        }

        public string Password
        {
            get
            {
                return Options.Password;
            }
            set
            {
                Options.Password = value;
            }
        }
	}
}
