using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.RiotData
{
    public partial class RiotQueue
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("numPlayersPerTeam")]
        public long NumPlayersPerTeam { get; set; }

        [JsonProperty("minimumQueueDodgeDelayTime")]
        public long MinimumQueueDodgeDelayTime { get; set; }

        [JsonProperty("minimumParticipantListSize")]
        public long MinimumParticipantListSize { get; set; }

        [JsonProperty("maximumParticipantListSize")]
        public long MaximumParticipantListSize { get; set; }

        [JsonProperty("gameTypeConfigId")]
        public long GameTypeConfigId { get; set; }

        [JsonProperty("minLevel")]
        public long MinLevel { get; set; }

        [JsonProperty("queueState")]
        public string QueueState { get; set; }

        [JsonProperty("thresholdEnabled")]
        public bool ThresholdEnabled { get; set; }

        [JsonProperty("thresholdSize")]
        public long ThresholdSize { get; set; }

        [JsonProperty("ranked")]
        public bool Ranked { get; set; }

        [JsonProperty("isForProduction")]
        public bool IsForProduction { get; set; }

        [JsonProperty("cacheName")]
        public string CacheName { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("supportedMapIds")]
        public long[] SupportedMapIds { get; set; }

        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        [JsonProperty("gameMutators")]
        public string[] GameMutators { get; set; }

        [JsonProperty("mapSelectionAlgorithm")]
        public string MapSelectionAlgorithm { get; set; }

        [JsonProperty("teamOnly")]
        public bool TeamOnly { get; set; }

        [JsonProperty("randomizeTeamSides")]
        public bool RandomizeTeamSides { get; set; }

        [JsonProperty("botsCanSpawnOnBlueSide")]
        public bool BotsCanSpawnOnBlueSide { get; set; }

        [JsonProperty("matchingThrottleConfig")]
        public MatchingThrottleConfig MatchingThrottleConfig { get; set; }

        [JsonProperty("blockedMinutesThreshold")]
        public long BlockedMinutesThreshold { get; set; }

        [JsonProperty("disallowFreeChampions")]
        public bool DisallowFreeChampions { get; set; }

        [JsonProperty("lastToggledOnTime")]
        public long LastToggledOnTime { get; set; }

        [JsonProperty("lastToggledOffTime")]
        public long LastToggledOffTime { get; set; }

        [JsonProperty("gameXPFactor")]
        public string GameXpFactor { get; set; }

        [JsonProperty("m_nDataVersion")]
        public long MNDataVersion { get; set; }
    }

    public partial class MatchingThrottleConfig
    {
        [JsonProperty("limit")] public long Limit { get; set; }

        [JsonProperty("matchingThrottleProperties")]
        public object[] MatchingThrottleProperties { get; set; }

        [JsonProperty("cacheName")] public string CacheName { get; set; }

        [JsonProperty("m_nDataVersion")] public long MnDataVersion { get; set; }
    }
}
