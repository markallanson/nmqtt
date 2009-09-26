/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://code.google.com/p/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using nMqtt.SampleApp.ViewModels;

namespace nMqtt.SampleApp.Views
{
    public partial class ConnectionView : View<ConnectionViewModel>
    {
		public ConnectionView ()
		{
			InitializeComponent();		
        }
		
		protected override void InitializeDataBinding()
		{
			this.serverCombo.DataSource = ViewModel.Servers;
			this.serverCombo.DataBindings.Add ("Text", ViewModel, "Server");

			this.portCombo.DataSource = ViewModel.Ports;
			this.portCombo.DataBindings.Add ("Text", ViewModel, "Port");
		}
		
		protected override void InitializeEventHandlers()
		{
			this.connectButton.Click += (sender, e) => ViewModel.Connect();
			this.disconnectButton.Click += (sender, e) => ViewModel.Disconnect();	
		}
    }
}
