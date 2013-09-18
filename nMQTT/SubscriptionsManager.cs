/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009-2013 Mark Allanson (mark@markallanson.net)
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Nmqtt
{
    /// <summary>
    /// A class that can manage the topic subscription process.
    /// </summary>
    internal class SubscriptionsManager : IDisposable
    {
        /// <summary>
        ///     used to synchronize access to subscriptions.
        /// </summary>
        private readonly object                           subscriptionPadlock        = new object();

        /// <summary>
        ///     Dispenser used for keeping track of subscription ids
        /// </summary>
        private readonly MessageIdentifierDispenser       messageIdentifierDispenser = new MessageIdentifierDispenser(); 

        /// <summary>
        ///     List of confirmed subscriptions, keyed on the topic name.
        /// </summary>
        private readonly Dictionary<string, Subscription> subscriptions               = new Dictionary<string, Subscription>();

        /// <summary>
        ///     A list of subscriptions that are pending acknowledgement, keyed on the message identifier.
        /// </summary>
        private readonly Dictionary<int, Subscription>    pendingSubscriptions        = new Dictionary<int, Subscription>();

        /// <summary>
        ///     The connection handler that we use to subscribe to subscription acknowledgements.
        /// </summary>
        private readonly IMqttConnectionHandler           connectionHandler;

        /// <summary>
        ///     Creates a new instance of a SubscriptionsManager that uses the specified connection to manage subscriptions. 
        /// </summary>
        /// <param name="connectionHandler">The connection handler that will be used to subscribe to topics.</param>
        public SubscriptionsManager(IMqttConnectionHandler connectionHandler) {
            this.connectionHandler = connectionHandler;
            this.connectionHandler.RegisterForMessage(MqttMessageType.SubscribeAck,   ConfirmSubscription);
            this.connectionHandler.RegisterForMessage(MqttMessageType.UnsubscribeAck, ConfirmUnsubscribe);
        }

        /// <summary>
        ///     Registers a new subscription with the subscription manager.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="qos"></param>
        /// <returns>The subscription message identifier.</returns>
        public IObservable<MqttReceivedMessage<T>> RegisterSubscription<T, TPayloadConverter>(string topic, MqttQos qos)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            // if we have a pending subscription or established subscription just return the existing observable.
            lock (subscriptionPadlock) {
                IObservable<MqttReceivedMessage<T>> existingObservable;
                if (TryGetExistingSubscription<T, TPayloadConverter>(topic, out existingObservable)) {
                    return existingObservable;
                }
                return CreateNewSubscription<T, TPayloadConverter>(topic, qos);
            }
        }

        /// <summary>
        ///     Gets a view on the existing observable, if the subscription already exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPayloadConverter"></typeparam>
        /// <param name="topic">The subscription topic to get.</param>
        /// <param name="observable">Set to an observable on the subscription if one exists, otherwise null.</param>
        /// <returns>True if an existing observable is available, otherwise false.</returns>
        private bool TryGetExistingSubscription<T, TPayloadConverter>(string topic, out IObservable<MqttReceivedMessage<T>> observable)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            var existingObservable = this.pendingSubscriptions.Values
                                                              .Union(this.subscriptions.Values)
                                                              .Where(ps => ps.Topic.Equals(topic))
                                                              .Select(ps => ps.Observable)
                                                              .FirstOrDefault();
            observable = null;
            if (existingObservable != null) {
                observable = WrapSubscriptionObservable<T, TPayloadConverter>(topic, existingObservable);
            } 
            return observable != null;
        }

        /// <summary>
        ///     Creates a new subscription for the specified topic.
        /// </summary>
        /// <typeparam name="T">The type of data the subscription is expected to return.</typeparam>
        /// <typeparam name="TPayloadConverter">The type of the converter that can convert from bytes to the type T.</typeparam>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <param name="qos">The QOS level to subscribe at.</param>
        /// <returns>An observable that yields messages when they arrive.</returns>
        private IObservable<MqttReceivedMessage<T>> CreateNewSubscription<T, TPayloadConverter>(string topic, MqttQos qos)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            // Add a pending subscription...
            var subject = new Subject<byte[]>();
            var sub = new Subscription {
                Topic             = topic,
                Qos               = qos,
                MessageIdentifier = messageIdentifierDispenser.GetNextMessageIdentifier("subscriptions"),
                CreatedTime       = DateTime.Now,
                Subject           = subject,
                Observable        = subject,
            };

            pendingSubscriptions.Add(sub.MessageIdentifier, sub);

            // build a subscribe message for the caller and send it off to the broker.
            var msg = new MqttSubscribeMessage().WithMessageIdentifier(sub.MessageIdentifier)
                                                .ToTopic(sub.Topic)
                                                .AtQos(sub.Qos);
            connectionHandler.SendMessage(msg);

            return WrapSubscriptionObservable<T, TPayloadConverter>(topic, sub.Observable);
        }

        /// <summary>
        ///     Wraps a raw byte array observable with the payload converter and yields a serialized messages in place.
        /// </summary>
        /// <typeparam name="T">The type of data the subscription is expected to return.</typeparam>
        /// <typeparam name="TPayloadConverter">The type of the converter that can convert from bytes to the type T.</typeparam>
        /// <param name="topic">The topic being wrapped</param>
        /// <param name="observable">The observable on the raw byte array to be wrapped.</param>
        /// <returns>An observable that yields MqttReceivedMessages of type T when a message arrives on the subscription.</returns>
        private static IObservable<MqttReceivedMessage<T>>  WrapSubscriptionObservable<T, TPayloadConverter>(string topic, IObservable<byte[]> observable) 
            where TPayloadConverter : IPayloadConverter<T>, new() {
            var payloadConverter = new TPayloadConverter();
            return observable.Select(ba => new MqttReceivedMessage<T>(topic, payloadConverter.ConvertFromBytes(ba)));
        }

        /// <summary>
        ///     Confirms a subscription has been made with the broker. Marks the sub as confirmed in the subs storage.
        /// </summary>
        /// <param name="msg">The message that triggered subscription confirmation.</param>
        /// <returns>True, always.</returns>
        private bool ConfirmSubscription(MqttMessage msg) {
            lock (subscriptionPadlock) {
                var subAck = (MqttSubscribeAckMessage)msg;
                Subscription sub;
                if (!pendingSubscriptions.TryGetValue(subAck.VariableHeader.MessageIdentifier, out sub)) {
                    throw new ArgumentException(
                        String.Format("There is no pending subscription against message identifier {0}",
                                      subAck.VariableHeader.MessageIdentifier));
                }

                // move it to the subscriptions pool, and out of the pending pool.
                subscriptions.Add(sub.Topic, sub);
                pendingSubscriptions.Remove(subAck.VariableHeader.MessageIdentifier);

                return true;
            }
        }

        /// <summary>
        ///     Cleans up after an unsubscribe message is received from the broker.
        /// </summary>
        /// <param name="msg">The unsubscribe message from the broker.</param>
        /// <returns>True, always.</returns>
        private bool ConfirmUnsubscribe(MqttMessage msg) {
            lock (subscriptionPadlock) {
                var unSubAck = (MqttUnsubscribeAckMessage)msg;
                var existingSubscription = subscriptions[unSubAck.VariableHeader.TopicName];
                if (existingSubscription != null) {
                    // Complete the observable sequence for the subscription, then remove it from the subscriptions.
                    // This is "proper" but may or may not be useful as we unsubscribe dynamically when the
                    // last of the subscribers has disposed themselves.
                    existingSubscription.Subject.OnCompleted();
                    subscriptions.Remove(existingSubscription.Topic);
                }
                return true;
            }
        }

        /// <summary>
        ///     Gets the current status of a subscription.
        /// </summary>
        /// <param name="topic">The topic to check the subscription for.</param>
        /// <returns>The current status of the subscription</returns>
        public SubscriptionStatus GetSubscriptionsStatus(string topic) {
            lock (subscriptionPadlock) {
                var status = SubscriptionStatus.DoesNotExist;
                if (subscriptions.ContainsKey(topic)) {
                    status = SubscriptionStatus.Active;
                }
                if (pendingSubscriptions.Any(pair => pair.Value.Topic.Equals(topic, StringComparison.Ordinal))) {
                    status = SubscriptionStatus.Pending;
                }
                return status;
            }
        }

        /// <summary>
        ///     Gets the subscription data method registered for a subscription topic.
        /// </summary>
        /// <param name="topic">The topic to retrieve the subscription data for.</param>
        /// <returns>The subscription data for a subscription, or null if there is no registered subscription.</returns>
        /// <remarks>
        ///     This will ignore pending subscriptions, so any messages that arrive for pending subscriptions will NOT be delivered. This
        ///     policy may change in the future if I find that some brokers might be a bit shifty. Sending messages to callbacks that
        ///     are not yet confirmed might not be handled gracefully by client consumers.
        /// </remarks>
        public Subscription GetSubscription(string topic) {
            Subscription subs;
            if (!subscriptions.TryGetValue(topic, out subs)) {
                return null;
            }
            return subs;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (connectionHandler != null) {
                connectionHandler.UnRegisterForMessage(MqttMessageType.SubscribeAck, ConfirmSubscription);
            }

            GC.SuppressFinalize(this);
        }
    }
}