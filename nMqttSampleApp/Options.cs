/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;

namespace nMqtt.SampleApp
{
	/// <summary>
	/// Basic static options class for storing options selected by the user.
	/// </summary>
	public static class Options
	{
		static Options()
		{
			ClientIdentifier = "nMqtt_Utility";
		}
	
		public static string ClientIdentifier 
		{
			get;
			set;
		}

        public static string Username
        {
            get;
            set;
        }

        public static string Password
        {
            get;
            set;
        }
	}
}
