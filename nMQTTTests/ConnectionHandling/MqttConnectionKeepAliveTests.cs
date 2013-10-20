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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nmqtt;

using Moq;
using Xunit;

namespace NmqttTests.ConnectionHandling
{
    public class MqttConnectionKeepAliveTests
    {
        [Fact]
        public void CtorRegistersForPingRequest()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.RegisterForMessage(MqttMessageType.PingRequest, It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);

            chMock.VerifyAll();
            ka.Dispose();
        }

        [Fact]
        public void CtorRegistersForPingResponse()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.RegisterForMessage(MqttMessageType.PingResponse, It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);

            chMock.VerifyAll();
            ka.Dispose();
        }

        [Fact]
        public void CtorRegistersForAllSentMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.RegisterForAllSentMessages(It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);

            chMock.VerifyAll();
            ka.Dispose();
        }

        [Fact]
        public void DisposeUnRegistersForPingRequest()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.UnRegisterForMessage(MqttMessageType.PingRequest, It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);
            ka.Dispose();

            chMock.VerifyAll();
        }

        [Fact]
        public void DisposeUnRegistersForPingResponse()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.UnRegisterForMessage(MqttMessageType.PingResponse, It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);
            ka.Dispose();

            chMock.VerifyAll();
        }

        [Fact]
        public void DisposeUnRegistersForAllSentMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to register and save the callback for later.
            chMock.Setup((x) => x.UnRegisterForAllSentMessages(It.IsAny<Func<MqttMessage, bool>>()));

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 60);
            ka.Dispose();

            chMock.VerifyAll();
        }

        [Fact]
        public void SendInactivityCausesPingRequest()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to Send
            chMock.Setup((x) => x.SendMessage(It.IsAny<MqttPingRequestMessage>()));

            // initiate the keepalive connection, then sleep for a few ms, to allow the timer to execute
            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 0);
            System.Threading.Thread.Sleep(1000);

            chMock.VerifyAll();
            ka.Dispose();
        }

        [Fact]
        public void SendPingRequestDoesNotOccurWhenOtherMessageSent()
        {
            Func<MqttMessage, bool> sendCallback = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to Send

            chMock.Setup((x) => x.SendMessage(It.IsAny<MqttPingRequestMessage>()));
            chMock.Setup((x) => x.RegisterForAllSentMessages(It.IsAny < Func<MqttMessage, bool>>()))
                .Callback((Func<MqttMessage, bool> regSendCallback) => sendCallback = regSendCallback);

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 1);
            System.Threading.Thread.Sleep(800);
            sendCallback(new MqttSubscribeAckMessage()); // fake that something was sent
            System.Threading.Thread.Sleep(300);

            // 1.3s should have passed, and we should not have had a send operation occur because we faked
            // a send operation to the callback.
            // Ie. SendMessage (first expectation we set on the mock) should not have been called.

            ka.Dispose();

            // verify it wasn't called.
            chMock.Verify((x) => x.SendMessage(It.IsAny<MqttPingRequestMessage>()), Times.Never());
        }


        /// <summary>
        /// Test that the receipt of mesages from a broker does not interfere with the transmission of ping
        /// requests by the keepalive - just because the broker is sending us messages doesn't mean it won't
        /// drop us if we don't send a ping.
        /// </summary>
        [Fact]
        public void PingReceiveDoesNotStopPingRequest()
        {
            Func<MqttMessage, bool> pingRespCallback = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            // mock the call to Send

            chMock.Setup((x) => x.SendMessage(It.IsAny<MqttPingRequestMessage>()));
            chMock.Setup((x) => x.RegisterForMessage(MqttMessageType.PingResponse, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pingRespCallback = cb);

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 1);
            System.Threading.Thread.Sleep(800);
            pingRespCallback(new MqttPingResponseMessage()); // fake that we received a ping response from the broker.
            System.Threading.Thread.Sleep(300);

            ka.Dispose();

            // verify that we did actually send a ping request.
            chMock.VerifyAll();
        }

        [Fact]
        public void PingRequestFromBrokerCausesPingResponse()
        {
            Func<MqttMessage, bool> pingReqCallback = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup((x) => x.SendMessage(It.IsAny<MqttPingResponseMessage>()));
            chMock.Setup((x) => x.RegisterForMessage(MqttMessageType.PingRequest, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pingReqCallback = cb);

            MqttConnectionKeepAlive ka = new MqttConnectionKeepAlive(chMock.Object, 1);
            pingReqCallback(new MqttPingRequestMessage());
            
            ka.Dispose();

            // verify that we did actually send a ping request.
            chMock.VerifyAll();
        }
    }
}
