using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.SummaryAggStats")]
    public class SummaryAggStats : RiotRtmpObject
    {
        [RtmpSharp("statsJson")]
        public object StatsJson { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("stats")]
        public SummaryAggStat[] Stats { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
