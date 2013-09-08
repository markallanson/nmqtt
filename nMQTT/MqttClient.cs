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
using System.Linq;
using Nmqtt.Diagnostics;

namespace Nmqtt
{
    /// <summary>
    ///     A client class for interacting with MQTT Data Packets
    /// </summary>
    public sealed class MqttClient : IDisposable
    {
        private readonly string server;

        /// <summary>
        ///     The remote server that this client will connect to.
        /// </summary>
        public string Server {
            get { return server; }
        }

        private readonly int port;

        /// <summary>
        ///     The port on the remote server that this client will connect to.
        /// </summary>
        public int Port {
            get { return port; }
        }

        private readonly string clientIdentifier;

        /// <summary>
        ///     Gets the Client Identifier of this instance of the MqttClient
        /// </summary>
        public string ClientIdentifier {
            get { return clientIdentifier; }
        }

        /// <summary>
        ///     Gets the current conneciton state of the Mqtt Client.
        /// </summary>
        public ConnectionState ConnectionState {
            get {
                if (connectionHandler != null) {
                    return connectionHandler.State;
                } else {
                    return Nmqtt.ConnectionState.Disconnected;
                }
            }
        }

        /// <summary>
        ///     The Handler that is managing the connection to the remote server.
        /// </summary>
        private MqttConnectionHandler connectionHandler;

        /// <summary>
        ///     The subscriptions manager responsible for tracking subscriptions.
        /// </summary>
        private SubscriptionsManager subscriptionsManager;

        /// <summary>
        ///     Handles the connection management while idle.
        /// </summary>
        private MqttConnectionKeepAlive keepAlive;

        /// <summary>
        ///     Handles everything to do with publication management.
        /// </summary>
        private PublishingManager publishingManager;

        /// <summary>
        ///     Handles the logging of received messages for diagnostic purpose.
        /// </summary>
        private MessageLogger messageLogger;

        /// <summary>
        ///     Event raised when a Message has been received from the wire.
        /// </summary>
        public event EventHandler<MqttMessageEventArgs> MessageAvailable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttClient" /> class using the default Mqtt Port.
        /// </summary>
        /// <param name="server">The server hostname to connect to.</param>
        /// <param name="clientIdentifier">The client identifier to use to connect with.</param>
        public MqttClient(string server, string clientIdentifier)
            : this(server, Constants.DefaultMqttPort, clientIdentifier) {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttClient" /> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        /// <param name="clientIdentifier">The ID that the broker can use to identify the client.</param>
        public MqttClient(string server, int port, string clientIdentifier) {
            this.server = server;
            this.port = port;
            this.clientIdentifier = clientIdentifier;
        }

        /// <summary>
        ///     Performs a synchronous connect to the message broker.
        /// </summary>
        public ConnectionState Connect() {
            MqttConnectMessage connectMessage = GetConnectMessage();
            connectionHandler = new SynchronousMqttConnectionHandler();

            // TODO: Get Get timeout from config or ctor or elsewhere.
            keepAlive = new MqttConnectionKeepAlive(connectionHandler, 30);
            subscriptionsManager = new SubscriptionsManager(connectionHandler);
            messageLogger = new MessageLogger(connectionHandler);
            publishingManager = new PublishingManager(connectionHandler, HandlePublishMessage);

            return connectionHandler.Connect(this.server, this.port, connectMessage);
        }

        /// <summary>
        ///     Gets a configured connect message.
        /// </summary>
        /// <returns>An MqttConnectMessage that can be used to connect to a message broker.</returns>
        private MqttConnectMessage GetConnectMessage() {
            return new MqttConnectMessage()
                .WithClientIdentifier(clientIdentifier)
                .KeepAliveFor(30)
                .StartClean();
        }

        /// <summary>
        ///     Handles the processing of messages arriving from the message broker.
        /// </summary>
        /// <param name="message">The message that was received from the broker.</param>
        private bool HandlePublishMessage(MqttMessage message) {
            var pubMsg = (MqttPublishMessage) message;
            Subscription subs = subscriptionsManager.GetSubscription(pubMsg.VariableHeader.TopicName);
            if (subs == null) {
                return false;
            }

            // pass it on to the event subscribers.
            OnMessageAvailable(pubMsg.VariableHeader.TopicName,
                               subs.DataProcessor.ConvertFromBytes(pubMsg.Payload.Message.ToArray()));
            return true;
        }

        /// <summary>
        ///     Subscribles the specified topic with a callback function that accepts the raw message data.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="qosLevel">The qos level.</param>
        /// <returns></returns>
        public short Subscribe(string topic, MqttQos qosLevel) {
            return Subscribe<PassThroughPublishDataConverter>(topic, qosLevel);
        }

        /// <summary>
        ///     Initiates a topic subscription request to the connected broker with a strongly typed data processor callback.
        /// </summary>
        /// <typeparam name="TPublishDataConverter">The type that implements TPublishDataConverter that can parse the data arriving on the subscription.</typeparam>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <param name="qosLevel">The qos level the message was published at.</param>
        /// <returns>
        ///     The identifier assigned to the subscription.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1004:GenericMethodsShouldProvideTypeParameter",
            Justification = "See method above for non generic implementation")]
        public short Subscribe<TPublishDataConverter>(string topic, MqttQos qosLevel)
            where TPublishDataConverter : IPublishDataConverter {
            if (connectionHandler.State != ConnectionState.Connected) {
                throw new ConnectionException(connectionHandler.State);
            }

            short messageIdentifier = subscriptionsManager.RegisterSubscription<TPublishDataConverter>(topic, qosLevel);
            return messageIdentifier;
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>The message identiier assigned to the message.</returns>
        public short PublishMessage(string topic, object data) {
            return PublishMessage<PassThroughPublishDataConverter>(topic, MqttQos.AtMostOnce, data);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="qos">The QOS level to publish the message at.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>The message identiier assigned to the message.</returns>
        public short PublishMessage(string topic, MqttQos qos, object data) {
            return PublishMessage<PassThroughPublishDataConverter>(topic, qos, data);
        }


        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <typeparam name="TDataConverter">The type of the data convert.</typeparam>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>
        ///     The message identiier assigned to the message.
        /// </returns>
        public short PublishMessage<TDataConverter>(string topic, object data)
            where TDataConverter : IPublishDataConverter {
            return PublishMessage<TDataConverter>(topic, MqttQos.AtMostOnce, data);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <typeparam name="TDataConverter">
        ///     The type of the data converter to use for converting the data
        ///     from the object to wire bytes.
        /// </typeparam>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="qualityOfService">The quality of service to attach to the message.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>
        ///     The message identiier assigned to the message.
        /// </returns>
        public short PublishMessage<TDataConverter>(string topic, MqttQos qualityOfService, object data)
            where TDataConverter : IPublishDataConverter {
            if (connectionHandler.State != ConnectionState.Connected) {
                throw new ConnectionException(connectionHandler.State);
            }

            return publishingManager.Publish<TDataConverter>(topic, qualityOfService, data);
        }

        private void OnMessageAvailable(string topic, object message) {
            if (MessageAvailable != null) {
                MessageAvailable(this, new MqttMessageEventArgs(topic, message));
            }
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (keepAlive != null) {
                keepAlive.Dispose();
            }

            if (subscriptionsManager != null) {
                subscriptionsManager.Dispose();
            }

            if (messageLogger != null) {
                messageLogger.Dispose();
            }

            if (connectionHandler != null) {
                connectionHandler.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}