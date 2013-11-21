using System.ComponentModel;
using nMqtt.SampleApp.Models;

namespace nMqtt.SampleApp.ViewsModels
{
    public class PublishMessageViewModel : ViewModel<PublishMessageModel>
    {
        public BindingList<string> Topics
        {
            get
            {
                return Model.Topics;
            }
            set
            {
                Model.Topics = value;
            }
        }

        public string Topic
        {
            get
            {
                return Model.Topic;
            }
            set
            {
                Model.Topic = value;
            }
        }

        public decimal Qos
        {
            get
            {
                return Model.Qos;
            }
            set
            {
                Model.Qos = (byte)value;
            }
        }

        public decimal QosMinimum
        {
            get
            {
                return 0;
            }
        }

        public decimal QosMaximum
        {
            get
            {
                return 2;
            }
        }

        public string Message
        {
            get
            {
                return Model.Message;
            }
            set
            {
                Model.Message = value;
                OnPropertyChanged("Message");
            }
        }

        public PublishMessageViewModel()
        {
            Model = new PublishMessageModel();
        }

        public void Publish()
        {
            Model.Publish();
            Message = string.Empty;
        }

        public void Publish(string fileName) {
            Model.Publish(fileName);
        }
    }
}