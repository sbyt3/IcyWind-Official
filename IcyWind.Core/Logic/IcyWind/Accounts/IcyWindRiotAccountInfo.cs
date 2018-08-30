using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.IcyWind.Accounts
{
    public class IcyWindRiotAccountInfo
    {
        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("sumName")]
        public string SumName { get; set; }

        [JsonProperty("sumLevel")]
        public int SumLevel { get; set; }

        [JsonProperty("sumIcon")]
        public int SumIcon { get; set; }

        [JsonProperty("pass")]
        public string Password { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }
    }
}
