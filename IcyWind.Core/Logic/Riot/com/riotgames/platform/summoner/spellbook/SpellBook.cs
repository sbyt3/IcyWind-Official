using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.spellbook
{
    [RtmpSharp("com.riotgames.platform.summoner.spellbook.SpellBookDTO")]
    public class SpellBook : RiotRtmpObject
    {
        [RtmpSharp("bookPagesJson")]
        public object BookPagesJson { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("bookPages")]
        public BookPage[] BookPages { get; set; }

        [RtmpSharp("dateString")]
        public string DateString { get; set; }

        [RtmpSharp("summonerId")]
        public int SummonerId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
