using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth.Data
{
    public class IcyWindAccountInfo
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public string Accounts { get; set; }

        public string IcywindRank { get; set; }

        public bool IsBanned { get; set; }

        public bool IsDev { get; set; }

        public bool IsPaid { get; set; }

        public bool IsPro { get; set; }

        public long PaidAmount { get; set; }

        public string UUID { get; set; }

        public string IPAddress { get; set; }

        [JsonProperty("err")]
        public string Error { get; set; }

        [JsonProperty("2fa")]
        public string TwoFactor { get; set; }
    }
}
