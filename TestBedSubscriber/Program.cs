using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nmqtt;
using System.Reactive.Linq.Observαble;


namespace TestBedSubscriber {
    class Program {
        static void Main(string[] args) {
            var client = new MqttClient("localhost", "whatter");
            client.Connect();

            var topicObservable = client.Observe<String, AsciiPayloadConverter>("Nmqtt_quickstart_topic",
                                                                                MqttQos.AtLeastOnce);
            topicObservable.Subscribe(
                msg => Console.WriteLine(String.Format("Msg Received on '{0}' is '{1}'", msg.Topic, msg.Payload)));
            topicObservable.Subscribe(
                msg => Console.WriteLine(String.Format("Second Msg Received on '{0}' is '{1}'", msg.Topic, msg.Payload)));


            Console.ReadKey();
        }
    }
}
