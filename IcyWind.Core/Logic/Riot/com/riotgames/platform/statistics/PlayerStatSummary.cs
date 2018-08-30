using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.PlayerStatSummary")]
    public class PlayerStatSummary : RiotRtmpObject
    {
        [RtmpSharp("maxRating")]
        public int MaxRating { get; set; }

        [RtmpSharp("playerStatSummaryTypeString")]
        public string PlayerStatSummaryTypeString { get; set; }

        [RtmpSharp("aggregatedStats")]
        public SummaryAggStats AggregatedStats { get; set; }

        [RtmpSharp("modifyDate")]
        public string ModifyDate { get; set; }

        [RtmpSharp("leaves")]
        public int Leaves { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("playerStatSummaryType")]
        public string PlayerStatSummaryType { get; set; }

        [RtmpSharp("userId")]
        public long UserId { get; set; }

        [RtmpSharp("losses")]
        public int Losses { get; set; }

        [RtmpSharp("rating")]
        public int Rating { get; set; }

        [RtmpSharp("aggregatedStatsJson")]
        public object AggregatedStatsJson { get; set; }

        [RtmpSharp("wins")]
        public int Wins { get; set; }
    }
}
