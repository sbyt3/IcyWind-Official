using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Auth.Data
{
    public class IcyWindRiotAccountCrypted
    {
        public string CryptString { get; set; }

        public string Region { get; set; }

        public long SumId { get; set; }

        public string SumUuid { get; set; }
    }
}
