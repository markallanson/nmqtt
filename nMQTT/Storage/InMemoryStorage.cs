using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Nmqtt.Storage
{
    /// <summary>
    /// Implements in memory storage of messages. 
    /// </summary>
    /// <remarks>
    /// Messages stored using in memory storage do not persist between client instances and therefore 
    /// should only be used when processing QoS level 0 (AtMostOnce) or 1 (AtLeastOnce) messages. In
    /// Memory storage could also be used if the clients were/are intended to always connect using
    /// the "CleanStart" mode.
    /// </remarks>
    internal class InMemoryStorage : IStorage
    {
        //Dictionary<short, MqttMessage> messages = new Dictionary<short, MqttMessage>();

        #region IStorage Members

        public void StoreMessage(MqttMessage message)
        {
        }

        public MqttMessage GetMessage(short messageId)
        {
            throw new NotImplementedException();
        }

        public void StoreSubscription(Subscription subs)
        {
            throw new NotImplementedException();
        }

        public Subscription GetSubscription(short subscriptionId)
        {
            throw new NotImplementedException();
        }

        public Collection<Subscription> GetSubscriptions()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
