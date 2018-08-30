using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Lobby
{
    public class CreatePartyService
    {
        [JsonProperty("accountId")]
        public long AccountId { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("currentParty")]
        public object CurrentParty { get; set; }

        [JsonProperty("eligibilityHash")]
        public long EligibilityHash { get; set; }

        [JsonProperty("parties")]
        public object Parties { get; set; }

        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("puuid")]
        public string Puuid { get; set; }

        [JsonProperty("registration")]
        public Registration Registration { get; set; }

        [JsonProperty("serverUtcMillis")]
        public long ServerUtcMillis { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public class Registration
    {
        [JsonProperty("inventoryToken")]
        public object InventoryToken { get; set; }

        [JsonProperty("inventoryTokens")]
        public string[] InventoryTokens { get; set; }

        [JsonProperty("leaguesTierRankToken")]
        public string LeaguesTierRankToken { get; set; }

        [JsonProperty("simpleInventoryToken")]
        public string SimpleInventoryToken { get; set; }

        [JsonProperty("summonerToken")]
        public string SummonerToken { get; set; }

        [JsonProperty("userInfoToken")]
        public string UserInfoToken { get; set; }
    }

    public class SetGameType
    {
        [JsonProperty("botDifficulty")]
        public object BotDifficulty { get; set; }

        [JsonProperty("gameCustomization")]
        public object GameCustomization { get; set; }

        [JsonProperty("gameType")]
        public string GameType { get; set; }

        [JsonProperty("maxPartySize")]
        public long MaxPartySize { get; set; }

        [JsonProperty("queueId")]
        public long QueueId { get; set; }
    }

    public class BodyHelper
    {
        public string body { get; set; }

        public string method { get; set; } = "PUT";

        public string url { get; set; }
    }
}
