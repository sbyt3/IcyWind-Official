using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.game.map;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.matchmaking;
using Newtonsoft.Json;
using RtmpSharp;
using RtmpSharp.IO.AMF3;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.systemstate
{
    [RtmpSharp("com.riotgames.platform.systemstate.ClientSystemStatesNotification")]
    public class ClientSystemStatesNotification : RiotRtmpObject, IExternalizable
    {
        [JsonIgnore]
        public ClientSystemStatesNotificationDecoded Data { get; set; }

        public void ReadExternal(IDataInput input)
        {
            int size = input.ReadByte() << 24 | input.ReadByte() << 16 | input.ReadByte() << 8 | input.ReadByte();
            string json = Encoding.UTF8.GetString(input.ReadBytes(size));

            Data = JsonConvert.DeserializeObject<ClientSystemStatesNotificationDecoded>(json);
        }

        public void WriteExternal(IDataOutput output)
        {
            string json = JsonConvert.SerializeObject(Data);
            byte[] b = Encoding.UTF8.GetBytes(json);
            output.WriteByte((byte)(b.Length >> 24));
            output.WriteByte((byte)(b.Length >> 16));
            output.WriteByte((byte)(b.Length >> 8));
            output.WriteByte((byte)(b.Length));
            output.WriteBytes(b);
        }
    }

    public class ClientSystemStatesNotificationDecoded
    {
        [JsonProperty("championTradeThroughLCDS")]
        public bool ChampionTradeThroughLcds { get; set; }

        [JsonProperty("practiceGameEnabled")]
        public bool PracticeGameEnabled { get; set; }

        [JsonProperty("advancedTutorialEnabled")]
        public bool AdvancedTutorialEnabled { get; set; }

        [JsonProperty("minNumPlayersForPracticeGame")]
        public long MinNumPlayersForPracticeGame { get; set; }

        [JsonProperty("practiceGameTypeConfigIdList")]
        public long[] PracticeGameTypeConfigIdList { get; set; }

        [JsonProperty("freeToPlayChampionIdList")]
        public long[] FreeToPlayChampionIdList { get; set; }

        [JsonProperty("freeToPlayChampionForNewPlayersIdList")]
        public long[] FreeToPlayChampionForNewPlayersIdList { get; set; }

        [JsonProperty("freeToPlayChampionsForNewPlayersMaxLevel")]
        public long FreeToPlayChampionsForNewPlayersMaxLevel { get; set; }

        [JsonProperty("inactiveChampionIdList")]
        public object[] InactiveChampionIdList { get; set; }

        [JsonProperty("gameModeToInactiveSpellIds")]
        public Dictionary<string, long[]> GameModeToInactiveSpellIds { get; set; }

        [JsonProperty("inactiveSpellIdList")]
        public object[] InactiveSpellIdList { get; set; }

        [JsonProperty("inactiveTutorialSpellIdList")]
        public long[] InactiveTutorialSpellIdList { get; set; }

        [JsonProperty("inactiveClassicSpellIdList")]
        public long[] InactiveClassicSpellIdList { get; set; }

        [JsonProperty("inactiveOdinSpellIdList")]
        public long[] InactiveOdinSpellIdList { get; set; }

        [JsonProperty("inactiveAramSpellIdList")]
        public long[] InactiveAramSpellIdList { get; set; }

        [JsonProperty("enabledQueueIdsList")]
        public long[] EnabledQueueIdsList { get; set; }

        [JsonProperty("unobtainableChampionSkinIDList")]
        public long[] UnobtainableChampionSkinIdList { get; set; }

        [JsonProperty("archivedStatsEnabled")]
        public bool ArchivedStatsEnabled { get; set; }

        [JsonProperty("queueThrottleDTO")]
        public object QueueThrottleDto { get; set; }

        [JsonProperty("gameMapEnabledDTOList")]
        public GameMapEnabledDtoList[] GameMapEnabledDtoList { get; set; }

        [JsonProperty("storeCustomerEnabled")]
        public bool StoreCustomerEnabled { get; set; }

        [JsonProperty("runeUniquePerSpellBook")]
        public bool RuneUniquePerSpellBook { get; set; }

        [JsonProperty("tribunalEnabled")]
        public bool TribunalEnabled { get; set; }

        [JsonProperty("observerModeEnabled")]
        public bool ObserverModeEnabled { get; set; }

        [JsonProperty("spectatorSlotLimit")]
        public long SpectatorSlotLimit { get; set; }

        [JsonProperty("clientHeartBeatRateSeconds")]
        public long ClientHeartBeatRateSeconds { get; set; }

        [JsonProperty("observableGameModes")]
        public string[] ObservableGameModes { get; set; }

        [JsonProperty("observableCustomGameModes")]
        public string ObservableCustomGameModes { get; set; }

        [JsonProperty("teamServiceEnabled")]
        public bool TeamServiceEnabled { get; set; }

        [JsonProperty("leagueServiceEnabled")]
        public bool LeagueServiceEnabled { get; set; }

        [JsonProperty("modularGameModeEnabled")]
        public bool ModularGameModeEnabled { get; set; }

        [JsonProperty("riotDataServiceDataSendProbability")]
        public double RiotDataServiceDataSendProbability { get; set; }

        [JsonProperty("displayPromoGamesPlayedEnabled")]
        public bool DisplayPromoGamesPlayedEnabled { get; set; }

        [JsonProperty("masteryPageOnServer")]
        public bool MasteryPageOnServer { get; set; }

        [JsonProperty("maxMasteryPagesOnServer")]
        public long MaxMasteryPagesOnServer { get; set; }

        [JsonProperty("tournamentSendStatsEnabled")]
        public bool TournamentSendStatsEnabled { get; set; }

        [JsonProperty("tournamentShortCodesEnabled")]
        public bool TournamentShortCodesEnabled { get; set; }

        [JsonProperty("replayServiceAddress")]
        public string ReplayServiceAddress { get; set; }

        [JsonProperty("buddyNotesEnabled")]
        public bool BuddyNotesEnabled { get; set; }

        [JsonProperty("localeSpecificChatRoomsEnabled")]
        public bool LocaleSpecificChatRoomsEnabled { get; set; }

        [JsonProperty("replaySystemStates")]
        public ReplaySystemState ReplaySystemStates { get; set; }

        [JsonProperty("sendFeedbackEventsEnabled")]
        public bool SendFeedbackEventsEnabled { get; set; }

        [JsonProperty("knownGeographicGameServerRegions")]
        public string[] KnownGeographicGameServerRegions { get; set; }

        [JsonProperty("leaguesDecayMessagingEnabled")]
        public bool LeaguesDecayMessagingEnabled { get; set; }

        [JsonProperty("currentSeason")]
        public long CurrentSeason { get; set; }
    }

    public class GameMapEnabledDtoList
    {
        [JsonProperty("gameMapId")]
        public long GameMapId { get; set; }

        [JsonProperty("minPlayers")]
        public long MinPlayers { get; set; }
    }
    public class ReplaySystemState
    {
        [JsonProperty("replayServiceEnabled")]
        public bool ReplayServiceEnabled { get; set; }

        [JsonProperty("replayServiceAddress")]
        public string ReplayServiceAddress { get; set; }

        [JsonProperty("endOfGameEnabled")]
        public bool EndOfGameEnabled { get; set; }

        [JsonProperty("matchHistoryEnabled")]
        public bool MatchHistoryEnabled { get; set; }

        [JsonProperty("getReplaysTabEnabled")]
        public bool GetReplaysTabEnabled { get; set; }

        [JsonProperty("backpatchingEnabled")]
        public bool BackpatchingEnabled { get; set; }
    }
}
