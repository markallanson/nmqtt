namespace Nmqtt.ConnectionHandling.Authentication
{
    internal abstract class Authenticator : IAuthenticator
    {
        public abstract void ConfigureAuthentication(MqttConnectMessage message);
    }
}