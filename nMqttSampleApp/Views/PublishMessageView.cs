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

using nMqtt.SampleApp.ViewsModels;

namespace nMqtt.SampleApp.Views
{
    public partial class PublishMessageView : View<PublishMessageViewModel>
    {
        public PublishMessageView()
        {
            InitializeComponent();
        }

        protected override void InitializeDataBinding()
        {
            topicsCombo.DataSource = ViewModel.Topics;
            topicsCombo.DataBindings.Add("Text", ViewModel, "Topic");

            qosNumeric.DataBindings.Add("Maximum", ViewModel, "QosMaximum");
            qosNumeric.DataBindings.Add("Minimum", ViewModel, "QosMinimum");
            qosNumeric.DataBindings.Add("Value", ViewModel, "Qos");

            messageTextbox.DataBindings.Add("Text", ViewModel, "Message");
        }

        protected override void InitializeEventHandlers()
        {
            publishButton.Click += (sender, e) => ViewModel.Publish();            
        }
    }
}
