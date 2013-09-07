# nMQTT Quickstart
Code snippets to help you getting started with nMQTT.

## Connection
Connect to an MQTT server running on the local machine using the default mqtt port

    var client = new Nmqtt.MqttClient("localhost", "Nmqtt_quickstart");
    ConnectionState connectionState = client.Connect();

Create a client using a custom MQTT Port

    MqttClient client = new Nmqtt.MqttClient("localhost", 25000, "Nmqtt_quickstart");

Note that calls to the connect method are synchronous and do not return until the MQTT server has acknowledged
the connection. There is currently no Asynchronous connection model.
    

## Subscription
Subscribe to a topic in order to receive messages published to that topic, and request that the message is
received at most one time ([QOS Level](http://public.dhe.ibm.com/software/dw/webservices/ws-mqtt/mqtt-v3r1.html#qos-flows) 0). 
Note that this method returns raw object data to the event handler on the client (the payload of the mqtt messages received).
You are responsible for parsing that raw object data in any way you choose.

    client.MessageAvailable += (sender, e) => Console.WriteLine(e.Message);
    short subscriptionId = client.Subscribe("Nmqtt_quickstart_topic", MqttQos.AtMostOnce);

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

    short subscriptionId = client.Subscribe<AsciiPublishDataConverter>("Nmqtt_quickstart_topic", MqttQos.AtMostOnce);

Two implementations are provided out of the box, The ascii converter above, and a passthrough converter which
is the default if no converter is explicitly supplied. The passthrough converter just passed through byte arrays.

## Receiving Messages
The `MqttClient` raises the MessageReceived event every time a message arrives. The `MessageReceivedEventArgs`
includes the topic, 

    client.MessageAvailable += (sender, e) =>
    {
        Console.WriteLine(e.Topic);
        Console.WriteLine(e.Message);
    };

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