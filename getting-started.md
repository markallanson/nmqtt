# nMQTT Quickstart
Code snippets to help you getting started with nMQTT.

## Connection
Connect to an MQTT server running on the local machine using the default MQTT port.

    var client = new Nmqtt.MqttClient("localhost", "Nmqtt_quickstart");
    ConnectionState connectionState = client.Connect();

Create a client using a custom MQTT port.

    MqttClient client = new Nmqtt.MqttClient("localhost", 25000, "Nmqtt_quickstart");

Note that calls to the connect method are synchronous and do not return until the MQTT server has acknowledged
the connection. There is currently no Asynchronous connection model.
    

## Subscription
nMQTT models MQTT subscriptions as observable sequences of messages using the 
[Rx Framework](http://msdn.microsoft.com/en-us/data/gg577609.aspx). To subscribe to a topic simply
call the client's `ObserveTopic` method, which returns an `IObservable`.
 
    IObservable<MqttRecivedMessage<byte[]> observation 
		= client.ObserveTopic("Nmqtt_quickstart_topic", MqttQos.AtMostOnce);

You can also subscribe with a data converter that handles serialization of the messages to and from
the Mqtt payload. The sample below converts the raw mqtt data to and from ASCII strings.

    class AsciiPublishDataConverter : IPublishDataConverter
    {
        public object ConvertFromBytes(byte[] messageData)
        {
            return System.Text.Encoding.ASCII.GetString(messageData);
        }
        public byte[] ConvertToBytes(object data)
        {
            return System.Text.Encoding.ASCII.GetBytes((string)data);
        }
    }

    IObservable<MsqqReceivedMessage<string>> observable 
           = client.ObserveTopic<string, AsciiPublishDataConverter>("Nmqtt_quickstart_topic", MqttQos.AtMostOnce);

## Receiving Messages
You can receive messages from the observation by subscribing to the observation using standard Rx symantics.

    IDisposable subscription = observation.Subscribe(msg => Process(msg.Topic, msg.Payload));
			 
nMQTT makes no guarantees about the thread that the messages will be yielded on, therefore you should take 
the same care that you would under normal Rx circumstances by observing the messages on your expected thread.
For example, in a Windows Forms application, you could observe the messages on the main UI thread using
the Rx ObserveOn extension method.

    IDisposable subscription = observable.ObserveOn(SynchronizationContext.Current)
                                         .Subscribe(msg => Process(msg.Topic, msg.Payload));

You can subscribe more than one to the same observation, and each subscription will receive each message
that arrives on that topic.

    IDisposable subscription1 = observable.Subscribe(msg => Process(msg.Topic, msg.Payload));
    IDisposable subscription2 = observable.Subscribe(msg => DoSomethingElse(msg.Topic, msg.Payload));

## Chaining Subscription and Receipt
Rx Observables can be chained, so of course you can so it all in one shot.

    IDisposable subscription = client.ObserveTopic<string, AsciiPublishDataConverter>("Nmqtt_quickstart_topic", MqttQos.AtMostOnce)
                                     .ObserveOn(SynchronizationContext.Current)
												 .Subscribe(msg => Process(msg.Topic, msg.Payload));

## Unsubscribing
Once you no longer want to receive messages for a topic, simply dispose your subscription. 

    subscription.Dispose();

Once all subscribers for a specific topic have been disposed, nMQTT will unsubscribe from the topic 
on the broker.

## Publishing Messages
Publishing messages performed using the `Publish` method on the `MqttClient`

    byte[] messgageData = new byte[] { 1, 2, 3 };
    client.PublishMessage("Nmqtt_quickstart_topic", messageData);

Be default published messages use QOS level 0 (At most once). You can specify an alternate QOS level.

    client.PublishMessage("Nmqtt_quickstart_topic", MqttQos.AtLeastOnce, messageData);

You can also leave serialization up to a data converter instead of passing in a raw byte array.

    client.PublishMessage<AsciiPublishDataConverter>("Nmqtt_quickstart_topic", MqttQos.AtLeastOnce, "Hello World.");

## Disconnction
`MqttClient`'s cannot be used to connect and disconnect at will. Once you are finished you must dispose 
the client. 

    client.Dispose()

If you wish to continue later you would then need to create another client and issue your subscriptions again.

This model is rather inflexible and will an alternate connect/disconnect model will likely be introduced in
future versions.