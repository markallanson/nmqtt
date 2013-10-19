namespace Nmqtt.ConnectionHandling.Authentication
{
    internal interface IAuthenticator
    {
        void ConfigureAuthentication(MqttConnectMessage message);
    }
}
