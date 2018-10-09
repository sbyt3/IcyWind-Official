using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat.Auth
{
    internal class XRiotRSO : BaseAuth
    {
        public override string AuthMethod => "X-Riot-RSO";

        public XRiotRSO(ChatClient client) : base(client, client.AuthCred) { }

        public override void HandleAuth()
        {
            ChatClient.Client.SendString($"<auth xmlns=\"urn:ietf:params:xml:ns:xmpp-sasl\" mechanism=\"{AuthMethod}\">{AuthCred.RSO}</auth>");
        }
    }
}
