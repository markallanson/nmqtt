using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nmqtt
{
    /// <summary>
    /// Event arguments for the Data Available event fired by the MqttConnection class.
    /// </summary>
    public class DataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the data stream that contains the data to read from.
        /// </summary>
        /// <value>The data stream.</value>
        public Stream DataStream { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAvailableEventArgs"/> class.
        /// </summary>
        /// <param name="dataStream">The data stream containing the message available for processing.</param>
        public DataAvailableEventArgs(Stream dataStream)
        {
            this.DataStream = dataStream;
        }
    }
}
