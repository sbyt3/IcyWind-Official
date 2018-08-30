using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.boost
{
    [RtmpSharp("com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO")]
    public class SummonerActiveBoostsDTO : RiotRtmpObject
    {
        [RtmpSharp("xpBoostEndDate")]
        public long XpBoostEndDate { get; set; }

        [RtmpSharp("xpBoostPerWinCount")]
        public long XpBoostPerWinCount { get; set; }

        [RtmpSharp("xpLoyaltyBoost")]
        public long XpLoyaltyBoost { get; set; }

        [RtmpSharp("ipBoostPerWinCount")]
        public long IpBoostPerWinCount { get; set; }

        [RtmpSharp("ipLoyaltyBoost")]
        public long IpLoyaltyBoost { get; set; }

        [RtmpSharp("summonerId")]
        public long SummonerId { get; set; }

        [RtmpSharp("ipBoostEndDate")]
        public long IpBoostEndDate { get; set; }
    }
}
