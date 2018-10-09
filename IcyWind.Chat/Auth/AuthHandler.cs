using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IcyWind.Chat.Auth
{
    public class AuthHandler
    {
        private ChatClient _chatClient { get; }

        public AuthHandler(ChatClient client)
        {
            _chatClient = client;
        }

        public bool HandleAuth(XmlNode xmlNode)
        {

            var authHandled = false;

            foreach (var mechanism in xmlNode.ChildNodes)
            {
                var mechanismXml = (XmlNode)mechanism;

                //Allow custom auth methods to be added
                //If a custom auth method is added, override IcyWind.Chat's
                //built in auth methods
                if (_chatClient.AuthMethod != null)
                {
                    //Check that the auth method specified is the
                    //wanted auth method
                    if (mechanismXml.InnerText != _chatClient.AuthMethod.AuthMethod)
                    {
                        continue;
                    }
                    //Handle the auth
                    authHandled = true;
                    _chatClient.AuthMethod.HandleAuth();
                    break;
                }

                //Check if the auth method is supported
                //If not continue to the next auth method
                if (!SupportedSASLAuth(mechanismXml.InnerText))
                {
                    continue;
                }
                //Handle auth
                authHandled = true;
                var authHandler = GetSASLAuthHandler(mechanismXml.InnerText);
                authHandler.HandleAuth();
                break;
            }

            if (authHandled)
                return true;

            if (_chatClient.AuthMethod != null)
            {
                throw new AuthNotSupportedException("The auth method specified is not supported by the server");
            }

            throw new AuthNotSupportedException(
                "IcyWind.Chat does not support any of the SASL auth mechanisms of the server");
        }

        public BaseAuth GetSASLAuthHandler(string method)
        {
            switch (method)
            {
                case "X-Riot-RSO":
                    return new XRiotRSO(_chatClient);
                default:
                    throw new AuthNotSupportedException($"The SASL authentication ({method}) is not supported by IcyWind.Chat");
            }
        }

        public bool SupportedSASLAuth(string method)
        {
            switch (method)
            {
                case "X-Riot-RSO":
                    return true;
                default:
                    return false;
            }
        }
    }
}
