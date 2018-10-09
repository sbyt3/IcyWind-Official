using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat.Auth
{
    public class AuthNotSupportedException : Exception
    {
        public AuthNotSupportedException() { }

        public AuthNotSupportedException(string message) : base(message) { }

        public AuthNotSupportedException(string message, Exception inner) : base(message, inner) { }
    }
}
