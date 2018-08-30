using System;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.spellbook
{
    [RtmpSharp("com.riotgames.platform.summoner.spellbook.SpellBookPageDTO")]
    public class BookPage : RiotRtmpObject
    {
        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("pageId")]
        public int PageId { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("current")]
        public bool Current { get; set; }

        [RtmpSharp("slotEntries")]
        public SlotEntry[] SlotEntries { get; set; }

        [RtmpSharp("createDate")]
        public string CreateDate { get; set; }

        [RtmpSharp("summonerId")]
        public int SummonerId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
