using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.spellbook;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.AllSummonerData")]
    public class AllSummonerData : RiotRtmpObject
    {
        /// <summary>
        /// Useless. pointless shitty code, brought to you by riot
        /// Oh yea and Riot removed the LOGIC behind this so I don't know wtf you should do
        /// </summary>
        [RtmpSharp("spellBook")]
        public SpellBook SpellBook { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("summonerDefaultSpells")]
        public SummonerDefaultSpells SummonerDefaultSpells { get; set; }

        [RtmpSharp("summoner")]
        public Summoner Summoner { get; set; }

        [RtmpSharp("summonerLevelAndPoints")]
        public SummonerLevelAndPoints SummonerLevelAndPoints { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
