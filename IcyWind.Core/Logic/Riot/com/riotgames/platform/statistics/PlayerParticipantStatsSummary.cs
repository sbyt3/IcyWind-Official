using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.PlayerParticipantStatsSummary")]
    public class PlayerParticipantStatsSummary : RiotRtmpObject
    {
        [RtmpSharp("skinName")]
        public string SkinName { get; set; }

        [RtmpSharp("skinIndex")]
        public long SkinIndex { get; set; }

        [RtmpSharp("gameId")]
        public long GameId { get; set; }

        [RtmpSharp("profileIconId")]
        public long ProfileIconId { get; set; }

        [RtmpSharp("elo")]
        public long Elo { get; set; }

        [RtmpSharp("leaver")]
        public bool Leaver { get; set; }

        [RtmpSharp("leaves")]
        public long Leaves { get; set; }

        [RtmpSharp("teamId")]
        public long TeamId { get; set; }

        [RtmpSharp("statistics")]
        public RawStatDTO[] Statistics { get; set; }

        [RtmpSharp("eloChange")]
        public long EloChange { get; set; }

        [RtmpSharp("level")]
        public long Level { get; set; }

        [RtmpSharp("botPlayer")]
        public bool BotPlayer { get; set; }

        [RtmpSharp("userId")]
        public long UserId { get; set; }

        [RtmpSharp("spell2Id")]
        public long Spell2Id { get; set; }

        [RtmpSharp("losses")]
        public long Losses { get; set; }

        [RtmpSharp("summonerName")]
        public string SummonerName { get; set; }

        [RtmpSharp("championId")]
        public long ChampionId { get; set; }

        [RtmpSharp("wins")]
        public long Wins { get; set; }

        [RtmpSharp("spell1Id")]
        public long Spell1Id { get; set; }
    }
}
