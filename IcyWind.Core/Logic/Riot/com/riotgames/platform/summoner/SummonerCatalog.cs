using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.SummonerCatalog")]
    public class SummonerCatalog : RiotRtmpObject
    {
        [RtmpSharp("items")]
        public object Items { get; set; }

        [RtmpSharp("spellBookConfig")]
        public RuneSlot[] RuneSlot { get; set; }
    }
}
