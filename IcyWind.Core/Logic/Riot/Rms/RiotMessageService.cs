using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Rms
{
    public class RiotMessageService
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("ts")]
        public long Ts { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        [JsonProperty("successful")]
        public string Successful { get; set; }

        [JsonProperty("expire_ts")]
        public long ExpireTs { get; set; }

        [JsonProperty("enabled")]
        public string Enabled { get; set; }
    }
}
