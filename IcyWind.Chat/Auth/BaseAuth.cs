using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat.Auth
{
    public abstract class BaseAuth
    {
        internal ChatClient ChatClient { get; }

        internal AuthCred AuthCred { get; }

        public abstract void HandleAuth();

        protected BaseAuth(ChatClient client, AuthCred cred)
        {
            ChatClient = client;
            AuthCred = cred;
        }
    }
}
