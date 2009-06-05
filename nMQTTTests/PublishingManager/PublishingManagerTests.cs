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

using Nmqtt;

using Xunit;
using Moq;
using System.Reflection;

namespace NmqttTests.PublishingManager
{
    public class PublishingManagerTests
    {
        [Fact]
        public void CtorRegistersForPublishMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });

            chMock.VerifyAll();
        }

        [Fact]
        public void CtorRegistersForPublishAckMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishAck, It.IsAny<Func<MqttMessage, bool>>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });

            chMock.VerifyAll();
        }

        [Fact]
        public void CtorRegistersForPublisCompletehMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishComplete, It.IsAny<Func<MqttMessage, bool>>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });

            chMock.VerifyAll();
        }

        [Fact]
        public void CtorRegistersForPublishReceivedMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishReceived, It.IsAny<Func<MqttMessage, bool>>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });

            chMock.VerifyAll();
        }

        [Fact]
        public void CtorRegistersForPublishReleaseMessages()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishRelease, It.IsAny<Func<MqttMessage, bool>>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });

            chMock.VerifyAll();
        }

        /*
         * 
         * Tests for the publishing of messages from the client to a remote party.
         * 
         */

        [Fact]
        public void PublishSendsPublishMessageThroughConnectionHandler()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()));

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });
            pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");

            chMock.VerifyAll();
        }

        [Fact]
        public void PublishSendsPublishMessageWithCorrectQos()
        {
            MqttPublishMessage pubMsg = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()))
                .Callback((MqttMessage msg) => pubMsg = (MqttPublishMessage)msg);

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtLeastOnce, "test");

            chMock.VerifyAll();

            // check the message QOS was correct
            Assert.Equal<MqttQos>(MqttQos.AtLeastOnce, pubMsg.Header.Qos);
        }

        [Fact]
        public void PublishSendsPublishMessageWithCorrectTopic()
        {
            MqttPublishMessage pubMsg = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()))
                .Callback((MqttMessage msg) => pubMsg = (MqttPublishMessage)msg);

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");

            chMock.VerifyAll();

            // check the message topic was correct
            Assert.Equal<string>("A/Topic", pubMsg.VariableHeader.TopicName);
        }

        [Fact]
        public void PublishSendsPublishMessageCorrectPayload()
        {
            MqttPublishMessage pubMsg = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()))
                .Callback((MqttMessage msg) => pubMsg = (MqttPublishMessage)msg);

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");

            chMock.VerifyAll();

            // check the message payload was correct
            Assert.Equal<string>("test", Encoding.ASCII.GetString(pubMsg.Payload.Message.ToArray<byte>()));
        }

        [Fact]
        public void PublishReturnIdMatchesPublishedMessageId()
        {
            MqttPublishMessage pubMsg = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()))
                .Callback((MqttMessage msg) => pubMsg = (MqttPublishMessage)msg);

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            int retId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");

            chMock.VerifyAll();

            // check the message message ids match
            Assert.Equal<int>(pubMsg.VariableHeader.MessageIdentifier, retId);
        }

        [Fact]
        public void ConsequtivePublishOnSameTopicGetsNextMessageId()
        {
            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()));

            // publish and save the first id
            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            int firstMsgId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");
            int secondMsgId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtMostOnce, "test");

            chMock.VerifyAll();

            Assert.Equal<int>(firstMsgId + 1, secondMsgId);
        }


        [Fact]
        public void PublishQos1Or2SavesMessageInStorage()
        {
            var chMock = new Mock<IMqttConnectionHandler>();

            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });
            int msgId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtLeastOnce, "test");

            var msgs = GetPublishedMessages(pm);

            Assert.True(msgs.ContainsKey(msgId));
        }

        [Fact]
        public void AcknowledgedQos1MessageRemovedFromStorage()
        {
            Func<MqttMessage, bool> ackCallback = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishAck, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType msgtype, Func<MqttMessage, bool> cb) => { ackCallback = cb; });

            // send the message, verify we've stored it ok.
            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage pubMsg) => { return true; });
            var msgId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.AtLeastOnce, "test");
            var msgs = GetPublishedMessages(pm);
            Assert.True(msgs.ContainsKey(msgId));

            // now fake an acknowledgement of the message, and ensure it's been removed from storage.
            ackCallback(new MqttPublishAckMessage().WithMessageIdentifier(msgId));
            Assert.False(msgs.ContainsKey(msgId));
        }

        [Fact]
        public void ReleasedQos2MessageRemovedFromStorage()
        {
            Func<MqttMessage, bool> rcvdCallback = null;
            Func<MqttMessage, bool> compCallback = null;

            var chMock = new Mock<IMqttConnectionHandler>();
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishReceived, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType msgtype, Func<MqttMessage, bool> cb) => { rcvdCallback = cb; });
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishRelease, It.IsAny<Func<MqttMessage, bool>>()));
            chMock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishComplete, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType msgtype, Func<MqttMessage, bool> cb) => { compCallback = cb; });
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishMessage>()));
            chMock.Setup(x => x.SendMessage(It.IsAny<MqttPublishReleaseMessage>()));

            // send the message, verify we've stored it ok.
            var pm = new Nmqtt.PublishingManager(chMock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });
            var msgId = pm.Publish<AsciiPublishDataConverter>("A/Topic", MqttQos.ExactlyOnce, "test");
            var msgs = GetPublishedMessages(pm);
            Assert.True(msgs.ContainsKey(msgId));

            // verify the pub msg has send a publish message.
            chMock.Verify(x => x.SendMessage(It.IsAny<MqttPublishMessage>()));

            // fake a response from the other party saying Received, this should initiate a Release to the other party
            rcvdCallback(new MqttPublishReceivedMessage().WithMessageIdentifier(msgId));
            Assert.True(msgs.ContainsKey(msgId));

            // verify the pub msg has sent a publish release message.
            chMock.Verify(x => x.SendMessage(It.IsAny<MqttPublishReleaseMessage>()));

            // fake a response from the other party saying "Complete", this should remove our copy of the message locally.
            compCallback(new MqttPublishCompleteMessage().WithMessageIdentifier(msgId));
            Assert.False(msgs.ContainsKey(msgId));
        }

        /*
         * 
         * Tests for the publishing of messages from the remote party to the client.
         *
         */
        [Fact]
        public void ReceivedMessageAtQos0DoesNotStoreMessage()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.AtMostOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            var rcvdMsgs = GetReceivedMessages(pm);
            Assert.False(rcvdMsgs.ContainsKey(msgId));
        }

        [Fact]
        public void ReceivedMessageAtQos1DoesNotStoreMessage()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.AtLeastOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            var rcvdMsgs = GetReceivedMessages(pm);
            Assert.False(rcvdMsgs.ContainsKey(msgId));
        }

        [Fact]
        public void ReceivedMessageAtQos2StoresMessage()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.ExactlyOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            var rcvdMsgs = GetReceivedMessages(pm);
            Assert.True(rcvdMsgs.ContainsKey(msgId));
        }

        [Fact]
        public void ReceivedMessageAtQos0DoesNotRespondToRemoteParty()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.SendMessage(It.IsAny<MqttMessage>()));

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.AtMostOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            // verify nothing was sent by the pub mgr
            mock.Verify(x => x.SendMessage(It.IsAny<MqttMessage>()), Times.Never());
        }

        [Fact]
        public void ReceivedMessageAtQos1SendsPublishAcknowledgement()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.SendMessage(It.IsAny<MqttPublishAckMessage>()));

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.AtLeastOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            mock.VerifyAll();
        }

        [Fact]
        public void ReceivedMessageAtQos2SendsPublishReceived()
        {
            Func<MqttMessage, bool> pubCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.SendMessage(It.IsAny<MqttPublishReceivedMessage>()));

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.ExactlyOnce)
                .PublishData(new byte[] { 0, 1, 2 }));

            mock.VerifyAll();
        }

        [Fact]
        public void ReleasedMessageAtQos2SendsPublishComplete()
        {
            Func<MqttMessage, bool> pubCallback = null;
            Func<MqttMessage, bool> pubRelCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishReceived, It.IsAny<Func<MqttMessage, bool>>()));
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishRelease, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubRelCallback = cb);
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishComplete, It.IsAny<Func<MqttMessage, bool>>()));

            mock.Setup(x => x.SendMessage(It.IsAny<MqttPublishReceivedMessage>()));
            mock.Setup(x => x.SendMessage(It.IsAny<MqttPublishCompleteMessage>()));

            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            // fake a publish from a remote party and verify we sent a received back.
            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.ExactlyOnce)
                .PublishData(new byte[] { 0, 1, 2 }));
            mock.Verify(x => x.SendMessage(It.IsAny<MqttPublishReceivedMessage>()));

            // fake the publish release message from remote party and verify that a publishcomplete is sent.
            pubRelCallback(new MqttPublishReleaseMessage().WithMessageIdentifier(msgId));
            mock.Verify(x => x.SendMessage(It.IsAny<MqttPublishCompleteMessage>()));
        }


        [Fact]
        public void ReceivedPublishMessageAtQos2StoresMessage()
        {
            Func<MqttMessage, bool> pubCallback = null;
            Func<MqttMessage, bool> pubRelCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishRelease, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubRelCallback = cb);
            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            // fake a publish from a remote party and verify we sent a received back.
            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.ExactlyOnce)
                .PublishData(new byte[] { 0, 1, 2 }));
            var rcvdMsgs = GetReceivedMessages(pm);
            Assert.True(rcvdMsgs.ContainsKey(msgId));
        }

        [Fact]
        public void ReleasedMessageAtQos2RemovesMessageFromStorage()
        {
            Func<MqttMessage, bool> pubCallback = null;
            Func<MqttMessage, bool> pubRelCallback = null;
            short msgId = 1;

            var mock = new Mock<IMqttConnectionHandler>();
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.Publish, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubCallback = cb);
            mock.Setup(x => x.RegisterForMessage(MqttMessageType.PublishRelease, It.IsAny<Func<MqttMessage, bool>>()))
                .Callback((MqttMessageType mt, Func<MqttMessage, bool> cb) => pubRelCallback = cb);
            var pm = new Nmqtt.PublishingManager(mock.Object, (MqttPublishMessage dummyPubMsg) => { return true; });

            // fake a publish from a remote party and verify we sent a received back.
            pubCallback(new MqttPublishMessage()
                .WithMessageIdentifier(msgId)
                .ToTopic("A/Topic")
                .WithQos(MqttQos.ExactlyOnce)
                .PublishData(new byte[] { 0, 1, 2 }));
            var rcvdMsgs = GetReceivedMessages(pm);
            Assert.True(rcvdMsgs.ContainsKey(msgId));

            // fake the publish release message from remote party and verify that a publishcomplete is sent.
            pubRelCallback(new MqttPublishReleaseMessage().WithMessageIdentifier(msgId));
            Assert.False(rcvdMsgs.ContainsKey(msgId));
        }




        /// <summary>
        /// Gets the published messages inside a publishging manager.
        /// </summary>
        /// <param name="pubMgr"></param>
        /// <returns></returns>
        private Dictionary<int, MqttPublishMessage> GetPublishedMessages(Nmqtt.PublishingManager pubMgr)
        {
            // we need to crack open the publishing manager and access some privates
            FieldInfo fi = typeof(Nmqtt.PublishingManager).GetField("publishedMessages", ReflectionBindingConstants.NonPublicField);
            return (Dictionary<int, MqttPublishMessage>)fi.GetValue(pubMgr);
        }


        /// <summary>
        /// Gets the received messages inside a publishing manager.
        /// </summary>
        /// <param name="pubMgr"></param>
        /// <returns></returns>
        private Dictionary<int, MqttPublishMessage> GetReceivedMessages(Nmqtt.PublishingManager pubMgr)
        {
            // we need to crack open the publishing manager and access some privates
            FieldInfo fi = typeof(Nmqtt.PublishingManager).GetField("receivedMessages", ReflectionBindingConstants.NonPublicField);
            return (Dictionary<int, MqttPublishMessage>)fi.GetValue(pubMgr);
        }
    }
}
