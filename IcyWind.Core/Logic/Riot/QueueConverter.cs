using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Events;

namespace IcyWind.Core.Logic.Riot
{
    public static class QueueConverter
    {
        public static Dictionary<int, KeyValuePair<Map, string>> Converter = new Dictionary<int, KeyValuePair<Map, string>>
        {
            { 0,    new KeyValuePair<Map, string>(Map.Unknown,          "Custom Game") },
            { 72,   new KeyValuePair<Map, string>(Map.HowlingAbyss,     "1v1 Snowdown Showdown games") },
            { 73,   new KeyValuePair<Map, string>(Map.HowlingAbyss,     "2v2 Snowdown Showdown games") },
            { 75,   new KeyValuePair<Map, string>(Map.SummonerRift,     "6v6 Hexakill games") },
            { 76,   new KeyValuePair<Map, string>(Map.SummonerRift,     "Ultra Rapid Fire games") },
            { 78,   new KeyValuePair<Map, string>(Map.HowlingAbyss,     "One For All: Mirror Mode games") },
            { 83,   new KeyValuePair<Map, string>(Map.SummonerRift,     "Co-op vs AI Ultra Rapid Fire games") },
            { 98,   new KeyValuePair<Map, string>(Map.TwistedTreeline,  "6v6 Hexakill games") },
            { 100,  new KeyValuePair<Map, string>(Map.ButcherBridge,    "5v5 ARAM games") },
            { 310,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Nemesis games") },
            { 313,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Black Market Brawlers games") },
            { 317,  new KeyValuePair<Map, string>(Map.CrystalScar,      "Definitely Not Dominion games") },
            { 325,  new KeyValuePair<Map, string>(Map.SummonerRift,     "All Random games") },
            { 400,  new KeyValuePair<Map, string>(Map.SummonerRift,     "5v5 Draft Pick games") },
            { 420,  new KeyValuePair<Map, string>(Map.SummonerRift,     "5v5 Ranked Solo games") },
            { 430,  new KeyValuePair<Map, string>(Map.SummonerRift,     "5v5 Blind Pick games") },
            { 440,  new KeyValuePair<Map, string>(Map.SummonerRift,     "5v5 Ranked Flex games") },
            { 450,  new KeyValuePair<Map, string>(Map.HowlingAbyss,     "5v5 ARAM games") },
            { 460,  new KeyValuePair<Map, string>(Map.TwistedTreeline,  "3v3 Blind Pick games") },
            { 470,  new KeyValuePair<Map, string>(Map.TwistedTreeline,  "3v3 Ranked Flex games") },
            { 600,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Blood Hunt Assassin games") },
            { 610,  new KeyValuePair<Map, string>(Map.CosmicRuins,      "Dark Star: Singularity games") },
            { 700,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Clash games") },
            { 800,  new KeyValuePair<Map, string>(Map.TwistedTreeline,  "Co-op vs. AI Intermediate Bot games") },
            { 810,  new KeyValuePair<Map, string>(Map.TwistedTreeline,  "Co-op vs. AI Intro Bot games") },
            { 820,  new KeyValuePair<Map, string>(Map.TwistedTreeline,  "Co-op vs. AI Beginner Bot games") },
            { 830,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Co-op vs. AI Intro Bot games") },
            { 840,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Co-op vs. AI Beginner Bot games") },
            { 850,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Co-op vs. AI Intermediate Bot games") },
            { 900,  new KeyValuePair<Map, string>(Map.SummonerRift,     "ARURF games") },
            { 910,  new KeyValuePair<Map, string>(Map.CrystalScar,      "Ascension games") },
            { 920,  new KeyValuePair<Map, string>(Map.HowlingAbyss,     "Legend of the Poro King games") },
            { 940,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Nexus Siege games") },
            { 950,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Doom Bots Voting games") },
            { 960,  new KeyValuePair<Map, string>(Map.SummonerRift,     "Doom Bots Standard games") },
            { 980,  new KeyValuePair<Map, string>(Map.ValoranCityPark,  "Star Guardian Invasion: Normal games") },
            { 990,  new KeyValuePair<Map, string>(Map.ValoranCityPark,  "Star Guardian Invasion: Onslaught games") },
            { 1000, new KeyValuePair<Map, string>(Map.Overcharge,       "PROJECT: Hunters games") },
            { 1010, new KeyValuePair<Map, string>(Map.SummonerRift,     "Snow ARURF games") },
            { 1020, new KeyValuePair<Map, string>(Map.SummonerRift,     "One for All games") },
            { 1200, new KeyValuePair<Map, string>(Map.NexusBlitz,       "Nexus Blitz games") }
        };

        public static string MapToName(Map map)
        {
            try
            {

                var type = typeof(Map);
                var memInfo = type.GetMember(map.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                return ((DescriptionAttribute) attributes[0]).Description;
            }
            catch
            {
                return "UnknownMap";
            }
        }
    }

    public enum Map
    {
        [Description("UnknownMap")]
        Unknown = -1,
        [Description("The Crystal Scar")]
        CrystalScar = 8,
        [Description("Twisted Treeline")]
        TwistedTreeline = 10,
        [Description("Summoner\'s Rift")]
        SummonerRift = 11,
        [Description("Howling Abyss")]
        HowlingAbyss = 12,
        [Description("Butcher\'s Bridge")]
        ButcherBridge = 14,
        [Description("Cosmic Ruins")]
        CosmicRuins = 16,
        [Description("Valoran City Park")]
        ValoranCityPark = 18,
        [Description("Substructure 43")]
        Overcharge = 19,
        [Description("Odyssey: Extraction Map")]
        CrashSite = 19,
        [Description("Nexus Blitz")]
        NexusBlitz = 21
    }
}
