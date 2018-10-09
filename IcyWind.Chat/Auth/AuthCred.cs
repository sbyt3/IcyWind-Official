using System;
using System.Security;

namespace IcyWind.Chat.Auth
{
    public class AuthCred
    {
        public string Username { get; set; }

        [Obsolete("Use the SecurePassword for added security")]
        public string Password { get; set; }

        public SecureString SecurePassword { get; set; }

        public string RSO { get; set; }
    }
}
