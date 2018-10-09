using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

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
