using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.SummonerGameModeSpells")]
    public class SummonerGameModeSpells : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("spell2Id")]
        public int Spell2Id { get; set; }

        [RtmpSharp("spell1Id")]
        public int Spell1Id { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
