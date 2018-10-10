using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Injector.InjectSocket
{
    public class NonLoopbackIPAddressException : Exception
    {
        public NonLoopbackIPAddressException() { }

        public NonLoopbackIPAddressException(string msg) : base(msg) { }

        public NonLoopbackIPAddressException(string msg, Exception ex) : base(msg, ex) { }
    }
}
