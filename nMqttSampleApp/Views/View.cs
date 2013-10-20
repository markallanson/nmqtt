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

using System.Windows.Forms;

namespace nMqtt.SampleApp
{
	public abstract class View<TViewModel> : UserControl
		where TViewModel : ViewModel
	{
		private TViewModel viewModel;
		
		/// <summary>
		/// Gets or Sets the ViewModel that the view represents
		/// </summary>
		public TViewModel ViewModel 
		{
			get
			{ 
				return viewModel; 
			}
			set
			{
				viewModel = value;
				InitializeDataBinding ();
				InitializeEventHandlers();
			}
		}
		
		/// <summary>
		/// When implemented in an inherting class, initializes data binding between the View and the ViewModel
		/// </summary>
		protected abstract void InitializeDataBinding();
		
		/// <summary>
		/// When implemented in an inheriting class, initializes event handlers for the View. 
		/// </summary>
		protected abstract void InitializeEventHandlers();
	}
}
