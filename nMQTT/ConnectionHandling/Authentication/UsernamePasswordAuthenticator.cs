using System;

namespace Nmqtt.ConnectionHandling.Authentication
{
    /// <summary>
    /// Model to authenticate a connection with username and password
    /// </summary>
    internal class UsernamePasswordAuthenticator : Authenticator
    {
        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; private set; }
        
        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="username">provided username</param>
        /// <param name="password">provided password</param>
        public UsernamePasswordAuthenticator(string username, string password)
        {
            Username = username;
            Password = password;            
        }

        public override void ConfigureAuthentication(MqttConnectMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");

            message.AuthenticateAs(Username, Password);
        }
    }
}