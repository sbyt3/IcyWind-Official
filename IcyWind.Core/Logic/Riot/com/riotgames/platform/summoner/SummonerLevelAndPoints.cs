using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.SummonerLevelAndPoints")]
    public class SummonerLevelAndPoints : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("expPoints")]
        public int ExpPoints { get; set; }

        [RtmpSharp("expToNextLevel")]
        public int ExpToNextLevel { get; set; }

        [RtmpSharp("summonerLevel")]
        public int SummonerLevel { get; set; }

        [RtmpSharp("summonerId")]
        public int SummonerId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
