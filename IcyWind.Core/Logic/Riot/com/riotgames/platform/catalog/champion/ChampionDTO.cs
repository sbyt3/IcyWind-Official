using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.champion
{
    [RtmpSharp("com.riotgames.platform.catalog.champion.ChampionDTO")]
    public class ChampionDTO : RiotRtmpObject
    {
        [RtmpSharp("rankedPlayEnabled")]
        public bool RankedPlayEnabled { get; set; }

        [RtmpSharp("winCountRemaining")]
        public long WinCountRemaining { get; set; }

        [RtmpSharp("botEnabled")]
        public bool BotEnabled { get; set; }

        [RtmpSharp("endDate")]
        public long EndDate { get; set; }

        [RtmpSharp("freeToPlayReward")]
        public bool FreeToPlayReward { get; set; }

        [RtmpSharp("sources")]
        public object[] Sources { get; set; }

        [RtmpSharp("owned")]
        public bool Owned { get; set; }

        [RtmpSharp("purchased")]
        public long Purchased { get; set; }

        [RtmpSharp("championSkins")]
        public ChampionSkinDTO[] ChampionSkins { get; set; }

        [RtmpSharp("purchaseDate")]
        public long PurchaseDate { get; set; }

        [RtmpSharp("active")]
        public bool Active { get; set; }

        [RtmpSharp("championId")]
        public long ChampionId { get; set; }

        [RtmpSharp("freeToPlay")]
        public bool FreeToPlay { get; set; }
    }
}
