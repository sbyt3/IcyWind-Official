using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Update.IcyWind
{
    public class IcyWindVersionJson
    {
        [JsonProperty("latest")]
        public Version Latest { get; set; }

        [JsonProperty("versions")]
        public Version[] Versions { get; set; }
    }

    public class Version
    {
        [JsonProperty("core")]
        public string Core { get; set; }

        [JsonProperty("assists")]
        public string Assists { get; set; }

        [JsonProperty("latest")]
        public bool LatestLatest { get; set; }
    }
}
