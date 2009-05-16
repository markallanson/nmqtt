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
using System.Linq;
using System.Text;
using NmqttConsole.Views;

namespace NmqttConsole.Presenters
{
    public class ShellPresenter
    {
        public ShellView View { get; set; }

        public ShellPresenter()
        {
            View = new ShellView();

            View.NewConnection += new EventHandler<EventArgs>(view_NewConnection);
        }

        /// <summary>
        /// Handle the user request for a new connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void view_NewConnection(object sender, EventArgs e)
        {
            // Create a new tab for the broker
            ConnectionPresenter presenter = new ConnectionPresenter();
            View.tcMessageBrokers.Items.Add(presenter.View);
            View.tcMessageBrokers.SelectedItem = presenter.View;
        }

        public void Show()
        {
            View.Show();
        }
    }
}
