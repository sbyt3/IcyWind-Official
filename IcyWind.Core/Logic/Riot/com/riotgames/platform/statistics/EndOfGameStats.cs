using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.EndOfGameStats")]
    public class EndOfGameStats : RiotRtmpObject
    {
        [RtmpSharp("talentPointsGained")]
        public long TalentPointsGained { get; set; }

        [RtmpSharp("battleBoostIpEarned")]
        public long BattleBoostIpEarned { get; set; }

        [RtmpSharp("ranked")]
        public bool Ranked { get; set; }

        [RtmpSharp("skinIndex")]
        public long SkinIndex { get; set; }

        [RtmpSharp("gameType")]
        public string GameType { get; set; }

        [RtmpSharp("causedEarlySurrender")]
        public bool CausedEarlySurrender { get; set; }

        [RtmpSharp("teamPlayerParticipantStats")]
        public PlayerParticipantStatsSummary[] TeamPlayerParticipantStats { get; set; }

        [RtmpSharp("reportGameId")]
        public long ReportGameId { get; set; }

        [RtmpSharp("difficulty")]
        public string Difficulty { get; set; }

        [RtmpSharp("gameLength")]
        public long GameLength { get; set; }

        [RtmpSharp("invalid")]
        public bool Invalid { get; set; }

        [RtmpSharp("gameEndedInEarlySurrender")]
        public bool GameEndedInEarlySurrender { get; set; }

        [RtmpSharp("roomName")]
        public string RoomName { get; set; }

        [RtmpSharp("userId")]
        public long UserId { get; set; }

        [RtmpSharp("coOpVsAiMinutesLeftToday")]
        public long CoOpVsAiMinutesLeftToday { get; set; }

        [RtmpSharp("otherTeamPlayerParticipantStats")]
        public PlayerParticipantStatsSummary[] OtherTeamPlayerParticipantStats { get; set; }

        [RtmpSharp("coOpVsAiMsecsUntilReset")]
        public long CoOpVsAiMsecsUntilReset { get; set; }

        [RtmpSharp("teamEarlySurrendered")]
        public bool TeamEarlySurrendered { get; set; }

        [RtmpSharp("newSpells")]
        public object[] NewSpells { get; set; }

        [RtmpSharp("gameId")]
        public long GameId { get; set; }

        [RtmpSharp("elo")]
        public long Elo { get; set; }

        [RtmpSharp("ipEarned")]
        public long IpEarned { get; set; }

        [RtmpSharp("roomPassword")]
        public string RoomPassword { get; set; }

        [RtmpSharp("eloChange")]
        public long EloChange { get; set; }

        [RtmpSharp("sendStatsToTournamentProvider")]
        public bool SendStatsToTournamentProvider { get; set; }

        [RtmpSharp("gameMode")]
        public string GameMode { get; set; }

        [RtmpSharp("earlySurrenderAccomplice")]
        public bool EarlySurrenderAccomplice { get; set; }

        [RtmpSharp("gameMutators")]
        public string[] GameMutators { get; set; }

        [RtmpSharp("queueType")]
        public string QueueType { get; set; }

        [RtmpSharp("summonerName")]
        public object SummonerName { get; set; }
    }
}
