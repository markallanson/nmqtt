/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2013 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using Common.Logging;
using Nmqtt.Diagnostics;

namespace Nmqtt
{
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

    /// <summary>
    ///     A client class for interacting with MQTT Data Packets
    /// </summary>
    public sealed class MqttClient : IDisposable
    {
        private static readonly ILog    Log = LogManager.GetCurrentClassLogger();

        private readonly string         server;
        private readonly int            port;
        private readonly string         clientIdentifier;

        /// <summary>
        ///     The Handler that is managing the connection to the remote server.
        /// </summary>
        private MqttConnectionHandler   connectionHandler;

        /// <summary>
        ///     The subscriptions manager responsible for tracking subscriptions.
        /// </summary>
        private SubscriptionsManager    subscriptionsManager;

        /// <summary>
        ///     Handles the connection management while idle.
        /// </summary>
        private MqttConnectionKeepAlive keepAlive;

        /// <summary>
        ///     Handles everything to do with publication management.
        /// </summary>
        private PublishingManager       publishingManager;

        /// <summary>
        ///     Handles the logging of received messages for diagnostic purpose.
        /// </summary>
        private MessageLogger           messageLogger;

        /// <summary>
        ///     The remote server that this client will connect to.
        /// </summary>
        public string                   Server {
            get { return server; }
        }

        /// <summary>
        ///     The port on the remote server that this client will connect to.
        /// </summary>
        public int                      Port {
            get { return port; }
        }

        /// <summary>
        ///     Gets the Client Identifier of this instance of the MqttClient
        /// </summary>
        public string                   ClientIdentifier {
            get { return clientIdentifier; }
        }

        /// <summary>
        ///     Gets the current conneciton state of the Mqtt Client.
        /// </summary>
        public ConnectionState          ConnectionState {
            get {
                return connectionHandler != null ? connectionHandler.State : ConnectionState.Disconnected;
            }
        }

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
            Log.Debug(m => m("Creating MqttClient for broker '{0}', port '{1}' using Client Identifier '{2}'",
                server, port, clientIdentifier));

            this.server = server;
            this.port = port;
            this.clientIdentifier = clientIdentifier;
        }

        /// <summary>
        /// Performs a synchronous connect to the message broker with an optional username and password
        /// for the purposes of authentication.
        /// </summary>
        /// <param name="username">Optionally the username to authenticate as.</param>
        /// <param name="password">Optionally the password to authenticate with.</param>
        public ConnectionState Connect(string username = null, string password = null)
        {
            Log.Debug(m => m("Initiating connection to broker '{0}', port '{1}' using Client Identifier '{2}'",
                server, port, clientIdentifier));

            if (username != null) {
                Log.Info(m => m("Authenticating with username '{0}' and password '{0}'", username, password));
                if (username.Trim().Length > Constants.RecommendedMaxUsernamePasswordLength) {
                    Log.Warn(m => m("Username length ({0}) exceeds the max recommended in the MQTT spec. ", username.Trim().Length));
                }
                if (password != null && password.Trim().Length > Constants.RecommendedMaxUsernamePasswordLength) {
                    Log.Warn(m => m("Password length ({0}) exceeds the max recommended in the MQTT spec. ", password.Trim().Length));
                }
            }

            var connectMessage = GetConnectMessage(username, password);
            connectionHandler = new SynchronousMqttConnectionHandler();

            // TODO: Get Get timeout from config or ctor or elsewhere.
            keepAlive = new MqttConnectionKeepAlive(connectionHandler, 30);
            publishingManager    = new PublishingManager(connectionHandler);
            subscriptionsManager = new SubscriptionsManager(connectionHandler, publishingManager);
            messageLogger = new MessageLogger(connectionHandler);

            return connectionHandler.Connect(this.server, this.port, connectMessage);
        }

        /// <summary>
        ///     Gets a configured connect message.
        /// </summary>
        /// <returns>An MqttConnectMessage that can be used to connect to a message broker.</returns>
        private MqttConnectMessage GetConnectMessage(string username, string password)
        {
            var message = new MqttConnectMessage()
                .WithClientIdentifier(clientIdentifier)
                .KeepAliveFor(30)
                .AuthenticateAs(username, password)
                .StartClean();

            return message;
        }


        /// <summary>
        ///     Subscribles the specified topic with a callback function that accepts the raw message data.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="qosLevel">The qos level.</param>
        /// <returns></returns>
        /// <exception cref="InvalidTopicException">If a topic that does not meet the MQTT topic spec rules is provided.</exception>
        public IObservable<MqttReceivedMessage<byte[]>> ListenTo(string topic, MqttQos qosLevel) {
            return ListenTo<byte[], PassThroughPayloadConverter>(topic, qosLevel);
        }

        /// <summary>
        ///     Initiates a topic subscription request to the connected broker with a strongly typed data processor callback.
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <param name="qosLevel">The qos level the message was published at.</param>
        /// <returns>
        ///     The identifier assigned to the subscription.
        /// </returns>
        /// <exception cref="InvalidTopicException">If a topic that does not meet the MQTT topic spec rules is provided.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "See method above for non generic implementation")]
        public IObservable<MqttReceivedMessage<T>> ListenTo<T, TPayloadConverter>(string topic, MqttQos qosLevel)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            if (connectionHandler.State != ConnectionState.Connected) {
                throw new ConnectionException(connectionHandler.State);
            }

            return subscriptionsManager.RegisterSubscription<T, TPayloadConverter>(topic, qosLevel);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>The message identiier assigned to the message.</returns>
        public short PublishMessage(string topic, byte[] data) {
            return PublishMessage<byte[], PassThroughPayloadConverter>(topic, MqttQos.AtMostOnce, data);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="qos">The QOS level to publish the message at.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>The message identiier assigned to the message.</returns>
        public short PublishMessage(string topic, MqttQos qos, byte[] data) {
            return PublishMessage<byte[], PassThroughPayloadConverter>(topic, qos, data);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <typeparam name="TPayloadConverter">The type of the data convert.</typeparam>
        /// <typeparam name="T">The Type of the data being published</typeparam>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>
        ///     The message identiier assigned to the message.
        /// </returns>
        public short PublishMessage<T, TPayloadConverter>(string topic, T data)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            return PublishMessage<T, TPayloadConverter>(topic, MqttQos.AtMostOnce, data);
        }

        /// <summary>
        ///     Publishes a message to the message broker.
        /// </summary>
        /// <typeparam name="TPayloadConverter">The type of the data converter to use.</typeparam>
        /// <typeparam name="T">The Type of the data being published</typeparam>
        /// <param name="topic">The topic to publish the message to.</param>
        /// <param name="qualityOfService">The quality of service to attach to the message.</param>
        /// <param name="data">The message to publish.</param>
        /// <returns>
        ///     The message identiier assigned to the message.
        /// </returns>
        /// <exception cref="InvalidTopicException">Thrown if the topic supplied violates the MQTT topic format rules.</exception>
        public short PublishMessage<T, TPayloadConverter>(string topic, MqttQos qualityOfService, T data)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            if (connectionHandler.State != ConnectionState.Connected) {
                throw new ConnectionException(connectionHandler.State);
            }

            try {
                var pubTopic = new PublicationTopic(topic);
                return publishingManager.Publish<T, TPayloadConverter>(pubTopic, qualityOfService, data);
            } catch (ArgumentException ex) {
                throw new InvalidTopicException(ex.Message, topic, ex);
            }
        }

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
        }
    }
// ReSharper restore UnusedMember.Global
// ReSharper restore MemberCanBePrivate.Global
}
