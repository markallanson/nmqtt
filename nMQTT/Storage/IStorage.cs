using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Nmqtt.Storage
{
    /// <summary>
    /// Interface that describes operations required for storage of publications when using Mqtt Qos
    /// levels of ExactlyOnce.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Stores the message into the backing store.
        /// </summary>
        /// <param name="message">The message to store.</param>
        void StoreMessage(MqttMessage message);

        /// <summary>
        /// Gets the message from the backing store.
        /// </summary>
        /// <param name="messageId">The message id of the message to retreive.</param>
        /// <returns></returns>
        MqttMessage GetMessage(short messageId);

        /// <summary>
        /// Stores a subscription
        /// </summary>
        /// <param name="subs">The subscription to store.</param>
        void StoreSubscription(Subscription subs);

        /// <summary>
        /// Gets a single subscription from it's ID.
        /// </summary>
        /// <param name="subscriptionId">The subscription id.</param>
        /// <returns></returns>
        Subscription GetSubscription(short subscriptionId);

        /// <summary>
        /// Gets all subscriptions.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification="In most cases this will be backed by durable storage")]
        Collection<Subscription> GetSubscriptions();
    }
}
