using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.BasePublicSummonerDTO")]
    public class BasePublicSummonerDTO : RiotRtmpObject
    {
        [RtmpSharp("internalName")]
        public string InternalName { get; set; }

        [RtmpSharp("previousSeasonHighestTier")]
        public string PreviousSeasonHighestTier { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("acctId")]
        public long AcctId { get; set; }

        [RtmpSharp("profileIconId")]
        public int ProfileIconId { get; set; }

        [RtmpSharp("puuid")]
        public string Puuid { get; set; }
    }
}
