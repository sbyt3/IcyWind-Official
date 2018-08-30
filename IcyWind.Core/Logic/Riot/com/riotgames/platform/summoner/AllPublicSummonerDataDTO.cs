using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.spellbook;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.AllPublicSummonerDataDTO")]
    public class AllPublicSummonerDataDTO : RiotRtmpObject
    {
        [RtmpSharp("spellBook")]
        public SpellBook SpellBook { get; set; }

        [RtmpSharp("summonerDefaultSpells")]
        public SummonerDefaultSpells SummonerDefaultSpells { get; set; }

        [RtmpSharp("summoner")]
        public BasePublicSummonerDTO Summoner { get; set; }

        [RtmpSharp("summonerLevelAndPoints")]
        public SummonerLevelAndPoints SummonerLevelAndPoints { get; set; }
    }
}
