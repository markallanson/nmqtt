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
