using IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.runes;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.spellbook
{
    [RtmpSharp("com.riotgames.platform.summoner.spellbook.SlotEntry")]
    public class SlotEntry : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("runeId")]
        public int? RuneId { get; set; }

        [RtmpSharp("runeSlotId")]
        public int? RuneSlotId { get; set; }

        [RtmpSharp("runeSlot")]
        public object RuneSlot { get; set; }
            
        /// <summary>
        /// Sometimes an empty array, sometimes this
        /// </summary>
        [RtmpSharp("rune")]
        public object Rune { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
