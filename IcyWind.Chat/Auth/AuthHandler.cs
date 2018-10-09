using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat.Auth
{
    public class AuthHandler
    {
        private ChatClient _chatClient { get; }

        public AuthHandler(ChatClient client)
        {
            _chatClient = client;
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
    }
}
