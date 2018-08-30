using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{

    [RtmpSharp("com.riotgames.platform.game.FeaturedGameInfo")]
    public class FeaturedGameInfo : RiotRtmpObject 
    {
        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("championVoteInfoList")]
        public object[] ChampionVoteInfoList { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
