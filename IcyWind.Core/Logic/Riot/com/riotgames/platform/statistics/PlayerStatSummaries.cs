using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics
{
    [RtmpSharp("com.riotgames.platform.statistics.PlayerStatSummaries")]
    public class PlayerStatSummaries : RiotRtmpObject
    {
        [RtmpSharp("season")]
        public int Season { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("playerStatSummarySet")]
        public PlayerStatSummary[] PlayerStatSummarySet { get; set; }

        [RtmpSharp("userId")]
        public long UserId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
