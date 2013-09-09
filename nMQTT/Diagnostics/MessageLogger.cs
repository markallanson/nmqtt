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
using Common.Logging;
using Nmqtt.Properties;

namespace Nmqtt.Diagnostics
{
    /// <summary>
    /// Implements message logging by observing the messages received and the messages sent.
    /// </summary>
    internal class MessageLogger : IDisposable
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private bool disposed;

        private readonly MqttConnectionHandler connectionHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessageLogger" /> class.
        /// </summary>
        /// <param name="connectionHandler">The connection handler.</param>
        public MessageLogger(MqttConnectionHandler connectionHandler) {
            if (Settings.Default.EnableMessageLogging) {
                this.connectionHandler = connectionHandler;
                // subscribe to ALL events received.
                foreach (MqttMessageType msgType in Enum.GetValues(typeof (MqttMessageType))) {
                    connectionHandler.RegisterForMessage(msgType, MessageLoggerCallback);
                }
                connectionHandler.RegisterForAllSentMessages(MessageSentCallback);
            }
        }

        /// <summary>
        ///     Called whenever a message is sent from the client to the broker.
        /// </summary>
        /// <param name="msg">The message that was sent.</param>
        /// <returns>true; always.</returns>
        private bool MessageSentCallback(MqttMessage msg) {
            LogMessage(msg, false);
            return true;
        }

        /// <summary>
        ///     Logs details of received messages.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <returns>true, always.</returns>
        private bool MessageLoggerCallback(MqttMessage msg) {
            LogMessage(msg, true);
            return true;
        }

        /// <summary>
        ///     Logs a message to the message log
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <param name="inbound">Set to true if the message is inbound to the client.</param>
        private void LogMessage(MqttMessage msg, bool inbound) {
            if (!disposed) {
                Log.Info(m => m(String.Format("{0} {1} ]>----<[ {2} ]>----|", inbound ? "<<<<" : ">>>>",
                                              DateTime.Now, 
                                              msg.Header.MessageType)));
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            disposed = true;

            // subscribe to ALL events received.
            foreach (MqttMessageType msgType in Enum.GetValues(typeof (MqttMessageType))) {
                connectionHandler.UnRegisterForMessage(msgType, MessageLoggerCallback);
            }
        }
    }
}