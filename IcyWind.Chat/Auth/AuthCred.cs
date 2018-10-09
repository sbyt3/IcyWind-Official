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

        public string Password { get; set; }

        public SecureString SecurePassword { get; set; }

        public string RSO { get; set; }
    }
}
