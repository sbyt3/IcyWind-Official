using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{
    [RtmpSharp("com.riotgames.platform.game.GameTypeConfigDTO")]
    public class GameTypeConfigDTO : RiotRtmpObject
    {
        [RtmpSharp("allowTrades")]
        public bool AllowTrades { get; set; }

        [RtmpSharp("banTimerDuration")]
        public int BanTimerDuration { get; set; }

        [RtmpSharp("maxAllowableBans")]
        public int MaxAllowableBans { get; set; }

        [RtmpSharp("crossTeamChampionPool")]
        public bool CrossTeamChampionPool { get; set; }

        [RtmpSharp("teamChampionPool")]
        public bool TeamChampionPool { get; set; }

        [RtmpSharp("postPickTimerDuration")]
        public int PostPickTimerDuration { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("banMode")]
        public string BanMode { get; set; }

        [RtmpSharp("id")]
        public int Id { get; set; }

        [RtmpSharp("duplicatePick")]
        public bool DuplicatePick { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("exclusivePick")]
        public bool ExclusivePick { get; set; }

        [RtmpSharp("mainPickTimerDuration")]
        public int MainPickTimerDuration { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("pickMode")]
        public string PickMode { get; set; }
    }
}
