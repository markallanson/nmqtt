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
            View.SubscribeRequested += new EventHandler<EventArgs>(View_SubscribeRequested);
        }

        void View_SubscribeRequested(object sender, EventArgs e)
        {
            mqttClient.Subscribe(View.tbSubTopicName.Text, MqttQos.AtMostOnce);
            View.lbSubscriptions.Items.Add(View.tbSubTopicName.Text);
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
            View.AppendStatusLine("Disconnected.");
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
            }
            else
            {
                mqttClient = new MqttClient(server, port, "nMqttClient");
            }

            mqttClient.PublishMessageReceived += mqttClient_PublishMessageReceived;
//            mqttClient.InvalidMessageReceived += mqttClient_InvalidMessageReceived;
            mqttClient.Connect();

            View.AppendStatusLine(
                String.Format("Connected to server {0} on port {1} with client identifier {2}",
                    mqttClient.Server, mqttClient.Port, mqttClient.ClientIdentifier));
        }

        void mqttClient_InvalidMessageReceived(object sender, InvalidMessageEventArgs e)
        {
            View.AppendStatusLine(String.Format("Invalid Message Data Received: {0}", e.Exception));
        }

        void mqttClient_PublishMessageReceived(object sender, PublishEventArgs e)
        {
            View.AppendStatusLine(e.GetAsAsciiString());
        }
    }
}
