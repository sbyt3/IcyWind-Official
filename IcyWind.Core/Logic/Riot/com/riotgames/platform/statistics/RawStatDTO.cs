using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.RawStatDTO")]
    public class RawStatDTO : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("value")]
        public long Value { get; set; }

        [RtmpSharp("statTypeName")]
        public string StatTypeName { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
