using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.runes
{
    [RtmpSharp("com.riotgames.platform.catalog.runes.Rune")]
    public class Rune : RiotRtmpObject
    {
        [RtmpSharp("imagePath")]
        public object ImagePath { get; set; }

        [RtmpSharp("toolTip")]
        public object ToolTip { get; set; }

        [RtmpSharp("tier")]
        public int Tier { get; set; }

        [RtmpSharp("itemId")]
        public int ItemId { get; set; }

        [RtmpSharp("runeType")]
        public RuneType RuneType { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("duration")]
        public int Duration { get; set; }

        [RtmpSharp("gameCode")]
        public int GameCode { get; set; }

        [RtmpSharp("itemEffects")]
        public object[] ItemEffects { get; set; }

        [RtmpSharp("baseType")]
        public string BaseType { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("description")]
        public string Description { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("uses")]
        public object Uses { get; set; }
    }
}
