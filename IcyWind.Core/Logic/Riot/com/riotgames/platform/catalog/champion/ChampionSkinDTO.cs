using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.champion
{
    [RtmpSharp("com.riotgames.platform.catalog.champion.ChampionSkinDTO")]
    public class ChampionSkinDTO : RiotRtmpObject
    {
        [RtmpSharp("lastSelected")]
        public bool LastSelected { get; set; }

        [RtmpSharp("stillObtainable")]
        public bool StillObtainable { get; set; }

        [RtmpSharp("purchaseDate")]
        public long PurchaseDate { get; set; }

        [RtmpSharp("winCountRemaining")]
        public long WinCountRemaining { get; set; }

        [RtmpSharp("endDate")]
        public long EndDate { get; set; }

        [RtmpSharp("championId")]
        public long ChampionId { get; set; }

        [RtmpSharp("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }

        [RtmpSharp("sources")]
        public object[] Sources { get; set; }

        [RtmpSharp("skinId")]
        public long SkinId { get; set; }

        [RtmpSharp("owned")]
        public bool Owned { get; set; }
    }
}
