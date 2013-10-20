using System;

namespace nMqtt.SampleApp
{
    public class TopicSubscribedEventArgs : EventArgs
    {
        public string Topic { get; private set; }

        public TopicSubscribedEventArgs(string topic)
        {
            Topic = topic;
        }
    }
}