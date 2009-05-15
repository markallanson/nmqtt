using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NmqttConsole.Views;
using Nmqtt;

namespace NmqttConsole.Presenters
{
    public class ConnectionPresenter
    {
        public ConnectionView View { get; set; }

        private MqttClient mqttClient;

        public ConnectionPresenter()
        {
            View = new ConnectionView();

            View.ConnectRequested += new EventHandler<EventArgs>(View_ConnectRequested);
        }

        void View_ConnectRequested(object sender, EventArgs e)
        {
            if (mqttClient != null && mqttClient.ConnectionState == ConnectionState.Connected)
            {
                Disconnect();
                View.btnConnect.Content = "Connect";
            }
            else
            {
                Connect();
                View.btnConnect.Content = "Disconnect";
            }
        }

        private void Disconnect()
        {
            mqttClient.Dispose();
            mqttClient = null;
        }

        private void Connect()
        {
            var serverPort = View.tbServerUrl.Text.Split(':');
            if (serverPort.Length == 0)
            {
                View.AppendStatusLine("Server connection string = <hostname> or <hostname>:<port>. If no port supplied assumed to be 1883");
            }

            string server = serverPort[0];
            int port = 0;

            if (serverPort.Length < 2)
            {
                View.AppendStatusLine("Using default port for connection");
            }
            else
            {
                port = Int32.Parse(serverPort[2]);
            }

            if (port == 0)
            {
                mqttClient = new MqttClient(server, "nMqttClient");
                View.AppendStatusLine("Connected to server " + server);
            }
            else
            {
                mqttClient = new MqttClient(server, port, "nMqttClient");
                View.AppendStatusLine("Connected to server " + server + " on port " + port);
            }

        }
    }
}
