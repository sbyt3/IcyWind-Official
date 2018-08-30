using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.runes
{
    [RtmpSharp("com.riotgames.platform.catalog.runes.RuneType")]
    public class RuneType : RiotRtmpObject
    {
        [RtmpSharp("runeTypeId")]
        public int RuneTypeId { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
