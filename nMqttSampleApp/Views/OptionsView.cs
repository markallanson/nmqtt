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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nMqtt.SampleApp.Views
{
    public partial class OptionsView : View<OptionsViewModel>
    {
        public OptionsView()
        {
            InitializeComponent();
        }
        
        protected override void InitializeDataBinding()
        {
            this.clientIdentifierTextBox.DataBindings.Add("Text", ViewModel, "ClientIdentifier");
            this.usernameTextbox.DataBindings.Add("Text", ViewModel, "Username");
            this.passwordTextbox.DataBindings.Add("Text", ViewModel, "Password");
        }
        
        protected override void InitializeEventHandlers ()
        {
        }
    }
}
