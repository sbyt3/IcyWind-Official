using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Lobby
{
    public class PartyPayload
    {
        [JsonProperty("httpStatus")]
        public long HttpStatus { get; set; }

        [JsonProperty("payload")]
        public Payload Payload { get; set; }
    }

    public class Payload
    {
        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("puuid")]
        public Guid Puuid { get; set; }

        [JsonProperty("accountId")]
        public long AccountId { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        [JsonProperty("eligibilityHash")]
        public long EligibilityHash { get; set; }

        [JsonProperty("parties")]
        public Party[] Parties { get; set; }

        [JsonProperty("currentParty")]
        public CurrentParty CurrentParty { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("serverUtcMillis")]
        public long ServerUtcMillis { get; set; }
    }

    public class CurrentParty
    {
        [JsonProperty("partyId")]
        public string PartyId { get; set; }

        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }

        [JsonProperty("players")]
        public Party[] Players { get; set; }

        [JsonProperty("chat")]
        public Chat Chat { get; set; }

        [JsonProperty("partyType")]
        public string PartyType { get; set; }

        [JsonProperty("activityLocked")]
        public bool ActivityLocked { get; set; }

        [JsonProperty("eligibilityRestrictions")]
        public object[] EligibilityRestrictions { get; set; }

        [JsonProperty("maxPartySize")]
        public long MaxPartySize { get; set; }

        [JsonProperty("eligibilityHash")]
        public long EligibilityHash { get; set; }
    }

    public class Chat
    {
        [JsonProperty("jid")]
        public string Jid { get; set; }
    }

    public class Party
    {
        [JsonProperty("puuid")]
        public Guid Puuid { get; set; }

        [JsonProperty("platformId")]
        public string PlatformId { get; set; }

        [JsonProperty("accountId")]
        public long AccountId { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        [JsonProperty("ready", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ready { get; set; }

        [JsonProperty("canInvite", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanInvite { get; set; }

        [JsonProperty("partyId")]
        public Guid PartyId { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
