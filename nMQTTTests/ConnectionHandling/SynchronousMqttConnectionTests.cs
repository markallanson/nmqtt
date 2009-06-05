using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using Nmqtt;

namespace NmqttTests.ConnectionHandling
{
    public class SynchronousMqttConnectionHandlerTests : IDisposable
    {
        MockBroker broker = null;
        string mockBrokerAddress = "localhost";
        int mockBrokerPort = 1883;
        string testClientId = "syncMqttTests";

        string nonExistantHostName = "aabbccddeeffeeddccbbaa.aa.bb";
        int badPort = 1884;

        public SynchronousMqttConnectionHandlerTests()
        {
            broker = new MockBroker();
        }

        #region IDisposable Members

        public void Dispose()
        {
            broker.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        [Fact]
        public void ConnectToBadHostNameThrowsConnectionException()
        {
            var ch = new SynchronousMqttConnectionHandler();
            Assert.Throws<ConnectionException>(() => ch.Connect(nonExistantHostName, mockBrokerPort, 
                new MqttConnectMessage().WithClientIdentifier(testClientId)));
        }

        [Fact]
        public void ConnectToBadHostNameInFaultedStateAfterConnectionException()
        {
            var ch = new SynchronousMqttConnectionHandler();
            Assert.Throws<ConnectionException>(() => ch.Connect(nonExistantHostName, mockBrokerPort,
                new MqttConnectMessage().WithClientIdentifier(testClientId)));
            Assert.Equal<ConnectionState>(ConnectionState.Faulted, ch.ConnectionState);
        }

        [Fact]
        public void ConnectToInvalidPortThrowsConnectionException()
        {
            var ch = new SynchronousMqttConnectionHandler();
            Assert.Throws<ConnectionException>(() => ch.Connect(mockBrokerAddress, badPort,
                new MqttConnectMessage().WithClientIdentifier(testClientId)));
        }

        [Fact]
        public void ConnectToInvalidPortInFaultedStateAfterConnectionException()
        {
            var ch = new SynchronousMqttConnectionHandler();
            Assert.Throws<ConnectionException>(() => ch.Connect(mockBrokerAddress, badPort,
                new MqttConnectMessage().WithClientIdentifier(testClientId)));
            Assert.Equal<ConnectionState>(ConnectionState.Faulted, ch.ConnectionState);
        }

        [Fact]
        public void ConnectWithNoConnectAckThrowsExceptionAndSetsConnectionToDisconnected()
        {
            var ch = new SynchronousMqttConnectionHandler();
            var ex = Assert.Throws<ConnectionException>(() => ch.Connect(mockBrokerAddress, mockBrokerPort,
                new MqttConnectMessage().WithClientIdentifier(testClientId)));
            Console.WriteLine("Exception Message Received {0}", ex.ToString());
            Assert.Equal<ConnectionState>(ConnectionState.Disconnected, ch.ConnectionState);
        }
    }
}
