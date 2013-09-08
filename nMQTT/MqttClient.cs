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
using Common.Logging;
using Nmqtt.Diagnostics;

namespace Nmqtt
{
    /// <summary>
    ///     A client class for interacting with MQTT Data Packets
    /// </summary>
    public sealed class MqttClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

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
            var subscription = subscriptionsManager.GetSubscription(pubMsg.VariableHeader.TopicName);
            if (subscription == null) {
                Log.WarnFormat("Recived message for a topic we're not subscribed to ({0})", pubMsg.VariableHeader.TopicName);
                return false;
            }

            var success = true;
            try {
                subscription.Subject.OnNext(pubMsg.Payload.Message.ToArray());
            } catch (Exception ex) {
                success = false;
                Log.Error(m => m("Error while publishing message to observer for topic {0}.", subscription.Topic), ex);
            }
            return success;
        }

        /// <summary>
        ///     Subscribles the specified topic with a callback function that accepts the raw message data.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="qosLevel">The qos level.</param>
        /// <returns></returns>
        public IObservable<MqttReceivedMessage<byte[]>> Observe(string topic, MqttQos qosLevel) {
            return Observe<byte[], PassThroughPayloadConverter>(topic, qosLevel);
        }

        /// <summary>
        ///     Initiates a topic subscription request to the connected broker with a strongly typed data processor callback.
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <param name="qosLevel">The qos level the message was published at.</param>
        /// <returns>
        ///     The identifier assigned to the subscription.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "See method above for non generic implementation")]
        public IObservable<MqttReceivedMessage<T>> Observe<T, TPayloadConverter>(string topic, MqttQos qosLevel)
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
        public short PublishMessage<T, TPayloadConverter>(string topic, MqttQos qualityOfService, T data)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            if (connectionHandler.State != ConnectionState.Connected) {
                throw new ConnectionException(connectionHandler.State);
            }

            return publishingManager.Publish<T, TPayloadConverter>(topic, qualityOfService, data);
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

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MqttReceivedMessage<T>
    {
        private readonly string topic;
        private readonly T      payload;

        /// <summary>
        /// The topic the message was received on.
        /// </summary>
        public string Topic {
            get { return topic; }
        }

        /// <summary>
        /// The payload of the mesage received.
        /// </summary>
        public T Payload {
            get { return payload; }
        }

        /// <summary>
        /// Initializes a new instance of an MqttReceivedMessage class.
        /// </summary>
        /// <param name="topic">The topic the message was received on</param>
        /// <param name="payload">The payload that was received.</param>
        internal MqttReceivedMessage(string topic, T payload) {
            this.topic = topic;
            this.payload = payload;
        }
    }
}