using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NmqttConsole.Views
{
    /// <summary>
    /// Interaction logic for ConnectionControl.xaml
    /// </summary>
    public partial class ConnectionView : TabItem
    {
        public ConnectionView()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectRequested != null)
            {
                ConnectRequested(this, new EventArgs());
            }
        }

        public event EventHandler<EventArgs> ConnectRequested;

        private void tbServerUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Header = tbServerUrl.Text;
        }

        /// <summary>
        /// Appends a line to the status.
        /// </summary>
        /// <param name="status">The status.</param>
        internal void AppendStatusLine(string status)
        {
            tblkStatus.Text += Environment.NewLine + status;
        }

        /// <summary>
        /// Appends the status string.
        /// </summary>
        /// <param name="status">The status.</param>
        internal void AppendStatus(string status)
        {
            tblkStatus.Text += status;
        }
    }
}
