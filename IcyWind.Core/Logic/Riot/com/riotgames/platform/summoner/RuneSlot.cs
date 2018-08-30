using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.runes;
using Newtonsoft.Json;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.RuneSlot")]
    public class RuneSlot : RiotRtmpObject
    {
        [RtmpSharp("id")]
        public int? Id { get; set; }

        [RtmpSharp("minLevel")]
        public int? MinLevel { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("runeType")]
        public RuneType RuneType { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
