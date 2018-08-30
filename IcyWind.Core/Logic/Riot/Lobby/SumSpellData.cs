using System.Collections.Generic;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Lobby
{
    public class SumSpellData
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("data")]
        public Dictionary<string, Datum> Data { get; set; }
    }

    public class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tooltip")]
        public string Tooltip { get; set; }

        [JsonProperty("maxrank")]
        public long MaxRank { get; set; }

        [JsonProperty("cooldown")]
        public long[] Cooldown { get; set; }

        [JsonProperty("cooldownBurn")]
        public string CooldownBurn { get; set; }

        [JsonProperty("cost")]
        public long[] Cost { get; set; }

        [JsonProperty("costBurn")]
        public string CostBurn { get; set; }

        [JsonProperty("datavalues")]
        public Datavalues DataValues { get; set; }

        [JsonProperty("effect")]
        public double[][] Effect { get; set; }

        [JsonProperty("effectBurn")]
        public string[] EffectBurn { get; set; }

        [JsonProperty("vars")]
        public Var[] Vars { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("summonerLevel")]
        public long SummonerLevel { get; set; }

        [JsonProperty("modes")]
        public string[] Modes { get; set; }

        [JsonProperty("costType")]
        public string CostType { get; set; }

        [JsonProperty("maxammo")]
        public string MaxAmmo { get; set; }

        [JsonProperty("range")]
        public long[] Range { get; set; }

        [JsonProperty("rangeBurn")]
        public long RangeBurn { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }
    }

    public partial class Datavalues
    {
    }

    public partial class Image
    {
        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("sprite")]
        public string Sprite { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("x")]
        public long X { get; set; }

        [JsonProperty("y")]
        public long Y { get; set; }

        [JsonProperty("w")]
        public long W { get; set; }

        [JsonProperty("h")]
        public long H { get; set; }
    }

    public partial class Var
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("coeff")]
        public object Coeff { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
