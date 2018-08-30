using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.SummonerDefaultSpells")]
    public class SummonerDefaultSpells : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("summonerDefaultSpellsJson")]
        public object SummonerDefaultSpellsJson { get; set; }

        [RtmpSharp("summonerDefaultSpellMap")]
        public object SummonerDefaultSpellMap { get; set; }

        [RtmpSharp("summonerId")]
        public int SummonerId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
