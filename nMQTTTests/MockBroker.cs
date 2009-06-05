using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace NmqttTests
{
    /// <summary>
    /// Mocks a broker, such as the RSMB, so that we can test the MqttConnection class, and some bits of the
    /// connection handlers that are difficult to test otherwise.
    /// </summary>
    public class MockBroker : IDisposable
    {
        int brokerPort = 1883;
        TcpListener listener = null;

        public MockBroker()
        {
            listener = new TcpListener(Dns.GetHostAddresses("localhost")[0], brokerPort);
            listener.Start();
        }

        #region IDisposable Members

        public void Dispose()
        {
            listener.Stop();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
