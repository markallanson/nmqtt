using System.ComponentModel;

namespace nMqtt.SampleApp.Models
{
    public interface IPublishMessageModel : IModel
    {
        BindingList<string> Topics
        {
            get; 
            set;
        }
        
        string Topic
        {
            get; 
            set;
        }

        byte Qos
        {
            get;
            set;
        }

        string Message
        {
            get; 
            set; 
        }

        void Publish();
    }
}