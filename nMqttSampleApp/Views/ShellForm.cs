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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using nMqtt.SampleApp.ViewModels;
using nMqtt.SampleApp.ViewsModels;

namespace nMqtt.SampleApp.Views
{
    public partial class ShellForm : Form
    {
        private ShellViewModel viewModel;

        public ShellForm(ShellViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitializeComponent();
			InitializeDataBindings();
			InitializeChildViews();
        }
		
		private void InitializeDataBindings()
		{
			this.DataBindings.Add("Text", viewModel, "WindowTitle");
		}
		
		private void InitializeChildViews()
		{
			this.connectionView1.ViewModel = new ConnectionViewModel();
			this.optionsView1.ViewModel = new OptionsViewModel();
			this.subscriptionView1.ViewModel = new SubscriptionViewModel();
            this.publishMessageView1.ViewModel = new PublishMessageViewModel();
		}
    }
}
