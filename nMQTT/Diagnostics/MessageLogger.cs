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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmqtt.Properties;
using System.IO;

namespace Nmqtt.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    internal class MessageLogger : IDisposable
    {
        private bool disposed;

        private MqttConnectionHandler connectionHandler;
        private StreamWriter logFileWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLogger"/> class.
        /// </summary>
        /// <param name="connectionHandler">The connection handler.</param>
        public MessageLogger(MqttConnectionHandler connectionHandler)
        {
            if (Settings.Default.EnableMessageLogging)
            {
                this.connectionHandler = connectionHandler;
                // subscribe to ALL events received.
                foreach (MqttMessageType msgType in Enum.GetValues(typeof(MqttMessageType)))
                {
                    connectionHandler.RegisterForMessage(msgType, MessageLoggerCallback);
                }
                connectionHandler.RegisterForAllSentMessages(MessageSentCallback);

                logFileWriter = new StreamWriter(Settings.Default.MessageLoggingFile);
            }
        }

        /// <summary>
        /// Called whenever a message is sent from the client to the broker.
        /// </summary>
        /// <param name="msg">The message that was sent.</param>
        /// <returns>true; always.</returns>
        private bool MessageSentCallback(MqttMessage msg)
        {
            LogMessage(msg, false);
            return true;
        }

        /// <summary>
        /// Logs details of received messages.
        /// </summary>
        /// <param name="msg">The message to log.</param>
        /// <returns>true, always.</returns>
        private bool MessageLoggerCallback(MqttMessage msg)
        {
            LogMessage(msg, true);
            return true;
        }

        /// <summary>
        /// Logs a message to the message log
        /// </summary>
        /// <param name="msg"></param>
        public void LogMessage(MqttMessage msg, bool inbound)
        {
            if (!disposed)
            {
                if (logFileWriter != null)
                {
                    logFileWriter.WriteLine(String.Format("{0} {1} ]>----<[ {2} ]>----|", inbound ? "<<<<" : ">>>>", DateTime.Now, msg.Header.MessageType));
                    logFileWriter.WriteLine(msg.ToString());
                    logFileWriter.Flush();
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            disposed = true;

            // subscribe to ALL events received.
            foreach (MqttMessageType msgType in Enum.GetValues(typeof(MqttMessageType)))
            {
                connectionHandler.UnRegisterForMessage(msgType, MessageLoggerCallback);
            }

            logFileWriter.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
