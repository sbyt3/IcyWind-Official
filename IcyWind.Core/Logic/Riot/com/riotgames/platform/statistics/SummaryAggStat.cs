using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.SummaryAggStat")]
    public class SummaryAggStat : RiotRtmpObject
    {
        [RtmpSharp("statType")]
        public string StatType { get; set; }

        [RtmpSharp("count")]
        public int Count { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("value")]
        public int Value { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
