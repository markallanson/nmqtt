using System;
using System.ComponentModel;

namespace nMqtt.SampleApp.Models
{
    public class PublishMessageModel : Model, IPublishMessageModel
    {
        public BindingList<string> Topics
        {
            get; 
            set;
        }
        
        public string Topic
        {
            get; 
            set;
        }

        public byte Qos
        {
            get;
            set;
        }

        public string Message
        {
            get; 
            set; 
        }

        public PublishMessageModel()
        {
            Topics = new BindingList<string>();
            MqttHandler.Instance.TopicSubscribed += OnTopicSubscribed;
        }

        private void OnTopicSubscribed(object sender, TopicSubscribedEventArgs e)
        {
            if (Topics.Contains(e.Topic)) return;
            
            Topics.Add(e.Topic);
            
            if (Topics.Count == 1)
            {
                Topic = e.Topic;
            }
        }

        public void Publish()
        {
           MqttHandler.Instance.Publish(Topic, Qos, Message);
        }
    }
}