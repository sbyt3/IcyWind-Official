using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.RiotData
{
    public class PerkHelper
    {
        [JsonProperty("perkIds")]
        public long[] PerkIds { get; set; }

        [JsonProperty("perkStyle")]
        public long PerkStyle { get; set; }

        [JsonProperty("perkSubStyle")]
        public long PerkSubStyle { get; set; }
    }
}
