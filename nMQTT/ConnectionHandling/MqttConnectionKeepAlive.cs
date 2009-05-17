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
using System.Threading;

namespace Nmqtt
{
    /// <summary>
    /// Implements keepalive functionality on the Mqtt Connection, ensuring that the connection
    /// remains active according to the keepalive seconds setting.
    /// </summary>
    /// <remarks>
    /// This class implements the keepalive by sending an MqttPingRequest to the broker if a message
    /// has not been send or received within the keepalive period.
    /// </remarks>
    internal sealed class MqttConnectionKeepAlive : IDisposable
    {
        private readonly int keepAlivePeriod;
        private MqttConnectionHandler connectionHandler;

        /// <summary>
        /// The threading timer that manages the ping callbacks.
        /// </summary>
        private Timer pingTimer;

        /// <summary>
        /// Used to synchronise shutdown and poing operations.
        /// </summary>
        private object shutdownPadlock = new object();
        
        /// <summary>
        /// Used to indicate that the class has been disposed and is awaiting GC.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MqttConnectionKeepAlive"/> class.
        /// </summary>
        /// <param name="connection">The connection to keep alive.</param>
        /// <param name="keepAliveSeconds">The keep alive duration in seconds.</param>
        public MqttConnectionKeepAlive(MqttConnectionHandler connectionHandler, int keepAliveSeconds)
        {
            this.connectionHandler = connectionHandler;
            this.keepAlivePeriod = keepAliveSeconds * 1000;
            
            // Start the timer so we do a ping whenever required.
            pingTimer = new Timer(PingRequired, null, keepAlivePeriod, keepAlivePeriod); 
        }

        /// <summary>
        /// Pings the message broker if there has been no activity for the specified amount of idle time.
        /// </summary>
        /// <param name="state"></param>
        private void PingRequired(object state)
        {
            // if we can't get the montor then the connection has been / is currently being disposed, so 
            // we don't want to do a ping (the connection handler might no longer be valid)
            if (Monitor.TryEnter(shutdownPadlock))
            {
                try
                {
                    MqttPingRequestMessage pingMsg = new MqttPingRequestMessage();
                    connectionHandler.SendMessage(pingMsg);
                }
                finally
                {
                    Monitor.Exit(shutdownPadlock);
                }
            }
        }

        /// <summary>
        /// Signal to the keepalive that a ping request has been received from the message broker.
        /// </summary>
        /// <remarks>
        /// The effect of calling this method on the keepalive handler is the transmission of a ping response
        /// message to the message broker on the current connection.
        /// </remarks>
        public void PingRequestReceived()
        {
            Monitor.Enter(shutdownPadlock);
            try
            {
                if (!disposed)
                {
                    MqttPingResponseMessage pingRespMessage = new MqttPingResponseMessage();
                    connectionHandler.SendMessage(pingRespMessage);

                    pingTimer.Change(keepAlivePeriod, keepAlivePeriod);
                }
            }
            finally
            {
                Monitor.Exit(shutdownPadlock);
            }
        }

        /// <summary>
        /// Signal to the keep alive handler that a message has been received.
        /// </summary>
        /// <remarks>
        /// Getting a signal that a message has been received means that we should reset our 
        /// ping timer and wait for a full duration before initiating the next ping.
        /// </remarks>
        public void MessageReceived()
        {
            pingTimer.Change(keepAlivePeriod, keepAlivePeriod);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // enter the monitor. If a ping is currently in progress, then wait for it complete before
            // disposing. Never release this monitor, we're going down anyway.
            Monitor.Enter(shutdownPadlock);
            this.disposed = true;

            if (pingTimer != null)
            {
                pingTimer.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
