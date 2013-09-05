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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace nMqtt.SampleApp.Views
{
    public partial class SubscriptionView : View<SubscriptionViewModel>
    {
        public SubscriptionView()
        {
            InitializeComponent();
        }
        
		protected override void InitializeDataBinding ()
		{
			topicsComboBox.DataSource = ViewModel.Topics;
			topicsComboBox.DataBindings.Add ("Text", ViewModel, "Topic");

			qosNumeric.DataBindings.Add("Maximum", ViewModel, "QosMaximum");
        		qosNumeric.DataBindings.Add("Minimum", ViewModel, "QosMinimum");
        		qosNumeric.DataBindings.Add("Value", ViewModel, "Qos");
        		
        		messageHistory.DataBindings.Add("Text", ViewModel, "MessageHistory");
        }

		protected override void InitializeEventHandlers()
		{
			subscribeButton.Click += (sender, e) => ViewModel.Subscribe();
		}
    }
}
