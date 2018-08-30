using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Lobby
{
    public class InventoryService
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("puuid")]
        public Guid Puuid { get; set; }

        [JsonProperty("accountId")]
        public long AccountId { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }

        [JsonProperty("items")]
        public Items Items { get; set; }

        [JsonProperty("itemsJwt")]
        public string ItemsJwt { get; set; }
    }
    public class Items
    {
    }
}
