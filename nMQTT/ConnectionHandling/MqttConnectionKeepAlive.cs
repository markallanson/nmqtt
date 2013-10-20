/* 
 * nMQTT, a .Net MQTT v3 client implementation.
 * http://wiki.github.com/markallanson/nmqtt
 * 
 * Copyright (c) 2009 Mark Allanson (mark@markallanson.net) & Contributors
 *
 * Licensed under the MIT License. You may not use this file except 
 * in compliance with the License. You may obtain a copy of the License at
 *
 *     http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Threading;

namespace Nmqtt
{
    /// <summary>
    ///     Implements keepalive functionality on the Mqtt Connection, ensuring that the connection
    ///     remains active according to the keepalive seconds setting.
    /// </summary>
    /// <remarks>
    ///     This class implements the keepalive by sending an MqttPingRequest to the broker if a message
    ///     has not been send or received within the keepalive period.
    /// </remarks>
    internal sealed class MqttConnectionKeepAlive : IDisposable
    {
        private readonly int keepAlivePeriod;

        private readonly IMqttConnectionHandler connectionHandler;

        /// <summary>
        ///     The threading timer that manages the ping callbacks.
        /// </summary>
        private readonly Timer pingTimer;

        /// <summary>
        ///     Used to synchronise shutdown and poing operations.
        /// </summary>
        private readonly object shutdownPadlock = new object();

        /// <summary>
        ///     Used to indicate that the class has been disposed and is awaiting GC.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MqttConnectionKeepAlive" /> class.
        /// </summary>
        /// <param name="connectionHandler">The connection to keep alive.</param>
        /// <param name="keepAliveSeconds">The keep alive duration in seconds.</param>
        public MqttConnectionKeepAlive(IMqttConnectionHandler connectionHandler, int keepAliveSeconds) {
            this.connectionHandler = connectionHandler;
            this.keepAlivePeriod = keepAliveSeconds*1000;

            // register for message handling of ping request and response messages.
            connectionHandler.RegisterForMessage(MqttMessageType.PingRequest, PingRequestReceived);
            connectionHandler.RegisterForMessage(MqttMessageType.PingResponse, PingResponseReceived);
            connectionHandler.RegisterForAllSentMessages(MessageSent);

            // Start the timer so we do a ping whenever required.
            pingTimer = new Timer(PingRequired, null, keepAlivePeriod, keepAlivePeriod);
        }

        /// <summary>
        ///     Handles the MessageSent event of the connectionHandler control.
        /// </summary>
        private bool MessageSent(MqttMessage msg) {
            if (!this.disposed) {
                pingTimer.Change(keepAlivePeriod, keepAlivePeriod);
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        ///     Pings the message broker if there has been no activity for the specified amount of idle time.
        /// </summary>
        /// <param name="state"></param>
        private void PingRequired(object state) {
            // if we can't get the montor then the connection has been / is currently being disposed, so 
            // we don't want to do a ping (the connection handler might no longer be valid)
            if (Monitor.TryEnter(shutdownPadlock)) {
                try {
                    var pingMsg = new MqttPingRequestMessage();
                    connectionHandler.SendMessage(pingMsg);
                } finally {
                    Monitor.Exit(shutdownPadlock);
                }
            }
        }

        /// <summary>
        ///     Signal to the keepalive that a ping request has been received from the message broker.
        /// </summary>
        /// <remarks>
        ///     The effect of calling this method on the keepalive handler is the transmission of a ping response
        ///     message to the message broker on the current connection.
        /// </remarks>
        private bool PingRequestReceived(MqttMessage pingMsg) {
            Monitor.Enter(shutdownPadlock);
            try {
                if (!disposed) {
                    var pingRespMessage = new MqttPingResponseMessage();
                    connectionHandler.SendMessage(pingRespMessage);

                    pingTimer.Change(keepAlivePeriod, keepAlivePeriod);
                }
            } finally {
                Monitor.Exit(shutdownPadlock);
            }

            return true;
        }

        /// <summary>
        ///     Processed ping response messages received from a message broker.
        /// </summary>
        /// <param name="pingMsg"></param>
        /// <returns></returns>
        private bool PingResponseReceived(MqttMessage pingMsg) {
            return true;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            // enter the monitor. If a ping is currently in progress, then wait for it complete before
            // disposing. Never release this monitor, we're going down anyway.
            Monitor.Enter(shutdownPadlock);
            this.disposed = true;

            if (connectionHandler != null) {
                connectionHandler.UnRegisterForMessage(MqttMessageType.PingRequest, PingRequestReceived);
                connectionHandler.UnRegisterForMessage(MqttMessageType.PingResponse, PingResponseReceived);
                connectionHandler.UnRegisterForAllSentMessages(MessageSent);
            }

            if (pingTimer != null) {
                pingTimer.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}