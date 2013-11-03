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
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Common.Logging;

namespace Nmqtt
{
    /// <summary>
    /// A class that can manage the topic subscription process.
    /// </summary>
    internal class SubscriptionsManager : IDisposable
    {
        private static readonly ILog                        Log                        = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     used to synchronize access to subscriptions.
        /// </summary>
        private readonly object                             subscriptionPadlock        = new object();

        /// <summary>
        ///     Dispenser used for keeping track of subscription ids
        /// </summary>
        private readonly MessageIdentifierDispenser         messageIdentifierDispenser = new MessageIdentifierDispenser(); 

        /// <summary>
        ///     List of confirmed subscriptions, keyed on the topic name.
        /// </summary>
        private readonly Dictionary<string, Subscription>   subscriptions              = new Dictionary<string, Subscription>();

        /// <summary>
        ///     A list of subscriptions that are pending acknowledgement, keyed on the message identifier.
        /// </summary>
        private readonly Dictionary<int, Subscription>      pendingSubscriptions       = new Dictionary<int, Subscription>();

        /// <summary>
        ///     The connection handler that we use to subscribe to subscription acknowledgements.
        /// </summary>
        private readonly IMqttConnectionHandler             connectionHandler;

        /// <summary>
        /// Publishing manager used for passing on published messages to subscribers.
        /// </summary>
        private readonly IPublishingManager                 publishingManager;

        /// <summary>
        ///     Creates a new instance of a SubscriptionsManager that uses the specified connection to manage subscriptions. 
        /// </summary>
        /// <param name="connectionHandler">The connection handler that will be used to subscribe to topics.</param>
        /// <param name="publishingManager">The punlishing manager that handles the receipt of messages from the broker.</param>
        public SubscriptionsManager(IMqttConnectionHandler connectionHandler,
                                    IPublishingManager     publishingManager) {
            this.connectionHandler = connectionHandler;
            this.publishingManager = publishingManager;
            this.connectionHandler.RegisterForMessage(MqttMessageType.SubscribeAck,   ConfirmSubscription);
            this.connectionHandler.RegisterForMessage(MqttMessageType.UnsubscribeAck, ConfirmUnsubscribe);
        }

        /// <summary>
        ///     Registers a new subscription with the subscription manager.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="qos"></param>
        /// <returns>An IObservable that yields any messages received once subscribed.</returns>
        /// <exception cref="InvalidTopicException">If a topic that does not meet the MQTT topic spec rules is provided.</exception>
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
        /// <param name="topic">The subscription Topic to get.</param>
        /// <param name="observable">Set to an observable on the subscription if one exists, otherwise null.</param>
        /// <returns>True if an existing observable is available, otherwise false.</returns>
        private bool TryGetExistingSubscription<T, TPayloadConverter>(string topic, out IObservable<MqttReceivedMessage<T>> observable)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            var existingObservable = this.pendingSubscriptions.Values
                                                              .Union(this.subscriptions.Values)
// ReSharper disable SuspiciousTypeConversion.Global 
// SubscriptionTopic implements explicit handling for string equality checking
                                                              .Where(ps => ps.Topic.Equals(topic))
// ReSharper restore SuspiciousTypeConversion.Global
                                                              .Select(ps => ps.Observable)
                                                              .FirstOrDefault();
            observable = null;
            if (existingObservable != null) {
                observable = WrapSubscriptionObservable<T, TPayloadConverter>(existingObservable);
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
        /// <exception cref="InvalidTopicException">If a topic that does not meet the MQTT topic spec rules is provided.</exception>
        private IObservable<MqttReceivedMessage<T>> CreateNewSubscription<T, TPayloadConverter>(string topic, MqttQos qos)
            where TPayloadConverter : IPayloadConverter<T>, new() {
            Log.Info(m => m("Creating subscription for topoc {0} @ QOS {1}.", topic, qos));

            try {
                var subscriptionTopic = new SubscriptionTopic(topic);

                // Get an ID that represents the subscription. We will use this same ID for unsubscribe as well.
                var msgId = messageIdentifierDispenser.GetNextMessageIdentifier("subscriptions");

                // create a new observable that is used to yield messages
                // that arrive for the topoc.
                var observable = CreateObservableForSubscription(subscriptionTopic, msgId);

                var sub = new Subscription {
                    Topic             = subscriptionTopic,
                    Qos               = qos,
                    MessageIdentifier = msgId,
                    CreatedTime       = DateTime.Now,
                    Observable        = observable,
                };

                pendingSubscriptions.Add(sub.MessageIdentifier, sub);

                // build a subscribe message for the caller and send it off to the broker.
                var msg = new MqttSubscribeMessage().WithMessageIdentifier(sub.MessageIdentifier)
                                                    .ToTopic(sub.Topic.ToString())
                                                    .AtQos(sub.Qos);
                connectionHandler.SendMessage(msg);

                return WrapSubscriptionObservable<T, TPayloadConverter>(sub.Observable);            
            } catch (ArgumentException ex) {
                Log.Warn(m => m("Error while processing topoc {0}. topoc structure not valid.", topic), ex);
                throw new InvalidTopicException(ex.Message, topic, ex);
            }
        }

        /// <summary>
        ///     Creates an observable for a subscription.
        /// </summary>
        /// <param name="subscriptionTopic">The topic to the obserbable should read messages on.</param>
        /// <param name="msgId">The messgeid assigned to the subscription.</param>
        /// <returns>An observable that yields a byte array for each message that arrives on a topoc.</returns>
        private IObservable<MqttReceivedMessage<byte[]>> CreateObservableForSubscription(SubscriptionTopic subscriptionTopic, short msgId) {
            var observable = Observable.Create<MqttReceivedMessage<byte[]>>(observer => {
                Log.Info(m => m("Creating underlying core observable for topoc {0}.", subscriptionTopic));

                // Listen for payload messages and when they arrive for our topoc
                // publish them onto the observable.
                var msgPubObservable
                    = Observable.FromEventPattern<PublishEventArgs>(h => publishingManager.MessageReceived += h,
                                                                    h => publishingManager.MessageReceived -= h);
                var msgPubSub = msgPubObservable
                    .Where(ep => subscriptionTopic.Matches(ep.EventArgs.Topic))
                    .Select(ep => ep.EventArgs)
                    .Subscribe(eventArgs => {
                        try {
                            // we use the messages topic name here so that if the 
                            // matched subscription was a wildcard, we give the 
                            // consumer the actual topic the message was published
                            // for, not the original wildcard subscription topic.
                            observer.OnNext(new MqttReceivedMessage<byte[]>(eventArgs.Topic.ToString(), 
                                                                            eventArgs.PublishMessage.Payload.Message.ToArray()));
                        } catch (Exception ex) {
                            Log.Error(m => m("Error while publishing message to observer for topic {0}.", eventArgs.Topic.ToString()), ex);
                        }
                    });

                // Unsubscribe from the topoc on the server,
                return Disposable.Create(() => {
                    Log.Info(m => m("Last subscriber gone for topic '{0}', unsubscribing on broker.", subscriptionTopic));
                    
                    // stop processing publish messages for this topoc received by thethe publishing manager.
                    msgPubSub.Dispose();

                    // build a unsubscribe message for the caller and send it off to the broker.
                    var unsubscribeMsg = new MqttUnsubscribeMessage()
                        .WithMessageIdentifier(messageIdentifierDispenser.GetNextMessageIdentifier("unsubscriptions"))
                        .WithMessageIdentifier(msgId)
                        .FromTopic(subscriptionTopic.ToString());
                    connectionHandler.SendMessage(unsubscribeMsg);
                });
            });

            // Publish and refcount so we can share the single subscription amongst all
            // subscribers and dispose automatically when everyone has disposed their
            // subscriptions.
            return observable.Publish()
                             .RefCount();
        }

        /// <summary>
        ///     Wraps a raw byte array observable with the payload converter and yields a serialized messages in place.
        /// </summary>
        /// <typeparam name="T">The type of data the subscription is expected to return.</typeparam>
        /// <typeparam name="TPayloadConverter">The type of the converter that can convert from bytes to the type T.</typeparam>
        /// <param name="observable">The observable on the raw byte array to be wrapped.</param>
        /// <returns>An observable that yields MqttReceivedMessages of type T when a message arrives on the subscription.</returns>
        private static IObservable<MqttReceivedMessage<T>>  WrapSubscriptionObservable<T, TPayloadConverter>(IObservable<MqttReceivedMessage<byte[]>> observable) 
            where TPayloadConverter : IPayloadConverter<T>, new() {
            var payloadConverter = new TPayloadConverter();
            return observable.Select(ba => new MqttReceivedMessage<T>(ba.Topic, payloadConverter.ConvertFromBytes(ba.Payload)));
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
                subscriptions.Add(sub.Topic.ToString(), sub);
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
                var existingSubscription = subscriptions.Values.FirstOrDefault(sub => sub.MessageIdentifier == unSubAck.VariableHeader.MessageIdentifier);
                if (existingSubscription != null) {
                    Log.Info(m => m("Unsubscription from broker topoc {0} acknowledged.", existingSubscription.Topic));

                    // Complete the observable sequence for the subscription, then remove it from the subscriptions.
                    // This is "proper" but may or may not be useful as we unsubscribe dynamically when the
                    // last of the subscribers has disposed themselves.
                    subscriptions.Remove(existingSubscription.Topic.ToString());
                } else {
                    Log.Warn(m => m("Unsubscribe ack recieved for unknown msgid {0}.", unSubAck.VariableHeader.MessageIdentifier));                    
                }
                return true;
            }
        }

        /// <summary>
        ///     Gets the current status of a subscription.
        /// </summary>
        /// <param name="topic">The topoc to check the subscription for.</param>
        /// <returns>The current status of the subscription</returns>  
        // ReSharper disable SuspiciousTypeConversion.Global
        public SubscriptionStatus GetSubscriptionsStatus(string topic) {
            lock (subscriptionPadlock) {
                var status = SubscriptionStatus.DoesNotExist;
                if (subscriptions.ContainsKey(topic)) {
                    status = SubscriptionStatus.Active;
                }
                if (pendingSubscriptions.Any(pair => pair.Value.Topic.Equals(topic))) {
                    status = SubscriptionStatus.Pending;
                }
                return status;
            }
        }
        // ReSharper restore SuspiciousTypeConversion.Global

        /// <summary>
        ///     Gets the subscription data method registered for a subscription topoc.
        /// </summary>
        /// <param name="topic">The topoc to retrieve the subscription data for.</param>
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