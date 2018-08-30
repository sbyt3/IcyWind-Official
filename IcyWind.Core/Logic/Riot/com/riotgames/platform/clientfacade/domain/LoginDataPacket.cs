using IcyWind.Core.Logic.Riot.com.riotgames.platform.broadcast;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.game;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.messaging.persistence;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.systemstate;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.clientfacade.domain
{
    [RtmpSharp("com.riotgames.platform.clientfacade.domain.LoginDataPacket")]
    public class LoginDataPacket : RiotRtmpObject
    {
        [RtmpSharp("restrictedGamesRemainingForRanked")]
        public int RestrictedGamesRemainingForRanked { get; set; }

        [RtmpSharp("playerStatSummaries")]
        public PlayerStatSummaries PlayerStatSummaries { get; set; }

        [RtmpSharp("restrictedChatGamesRemaining")]
        public int RestrictedChatGamesRemaining { get; set; }

        [RtmpSharp("minutesUntilShutdown")]
        public int MinutesUntilShutdown { get; set; }

        [RtmpSharp("minor")]
        public bool Minor { get; set; }

        [RtmpSharp("maxPracticeGameSize")]
        public int MaxPracticeGameSize { get; set; }

        [RtmpSharp("summonerCatalog")]
        public SummonerCatalog SummonerCatalog { get; set; }

        [RtmpSharp("ipBalance")]
        public int IpBalance { get; set; }

        [RtmpSharp("reconnectInfo")]
        public object ReconnectInfo { get; set; }

        [RtmpSharp("languages")]
        public string[] Languages { get; set; }

        [RtmpSharp("simpleMessages")]
        public SimpleDialogMessage[] SimpleMessages { get; set; }

        [RtmpSharp("allSummonerData")]
        public AllSummonerData AllSummonerData { get; set; }

        [RtmpSharp("customMinutesLeftToday")]
        public int CustomMinutesLeftToday { get; set; }

        [RtmpSharp("displayPrimeReformCard")]
        public bool DisplayPrimeReformCard { get; set; }

        [RtmpSharp("platformGameLifecycleDTO")]
        public object PlatformGameLifecycleDto { get; set; }

        [RtmpSharp("coOpVsAiMinutesLeftToday")]
        public int CoOpVsAiMinutesLeftToday { get; set; }

        [RtmpSharp("bingeData")]
        public object BingeData { get; set; }

        [RtmpSharp("inGhostGame")]
        public bool InGhostGame { get; set; }

        [RtmpSharp("bingePreventionSystemEnabledForClient")]
        public bool BingePreventionSystemEnabledForClient { get; set; }

        [RtmpSharp("bannedUntilDateMillis")]
        public int BannedUntilDateMillis { get; set; }

        [RtmpSharp("broadcastNotification")]
        public BroadcastNotification BroadcastNotification { get; set; }

        [RtmpSharp("minutesUntilMidnight")]
        public int MinutesUntilMidnight { get; set; }

        [RtmpSharp("coOpVsAiMsecsUntilReset")]
        public int CoOpVsAiMsecsUntilReset { get; set; }

        [RtmpSharp("clientSystemStates")]
        public ClientSystemStatesNotification ClientSystemStates { get; set; }

        [RtmpSharp("bingeMinutesRemaining")]
        public int BingeMinutesRemaining { get; set; }

        [RtmpSharp("leaverBusterPenaltyTime")]
        public int LeaverBusterPenaltyTime { get; set; }

        [RtmpSharp("platformId")]
        public string PlatformId { get; set; }

        [RtmpSharp("emailStatus")]
        public string EmailStatus { get; set; }

        [RtmpSharp("matchMakingEnabled")]
        public bool MatchMakingEnabled { get; set; }

        [RtmpSharp("minutesUntilShutdownEnabled")]
        public bool MinutesUntilShutdownEnabled { get; set; }

        [RtmpSharp("rpBalance")]
        public int RpBalance { get; set; }

        [RtmpSharp("showEmailVerificationPopup")]
        public bool ShowEmailVerificationPopup { get; set; }

        [RtmpSharp("bingeIsPlayerInBingePreventionWindow")]
        public bool BingeIsPlayerInBingePreventionWindow { get; set; }

        [RtmpSharp("gameTypeConfigs")]
        public GameTypeConfigDTO[] GameTypeConfigs { get; set; }

        [RtmpSharp("minorShutdownEnforced")]
        public bool MinorShutdownEnforced { get; set; }

        [RtmpSharp("competitiveRegion")]
        public string CompetitiveRegion { get; set; }

        [RtmpSharp("customMsecsUntilReset")]
        public int CustomMsecsUntilReset { get; set; }
    }
}
