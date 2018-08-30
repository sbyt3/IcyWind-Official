using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IcyWind.Core.Logic.Riot.RiotData
{
    public class RunesReforaged
    {
        [JsonProperty("page-settings")]
        public PageSettings PageSettings { get; set; }
    }

    public class PageSettings
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("schemaVersion")]
        public long SchemaVersion { get; set; }
    }

    public class Data
    {
        [JsonProperty("pages")]
        public object[] Pages { get; set; }

        [JsonProperty("perShardPerkBooks")]
        public PerShardPerkBooks PerShardPerkBooks { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }
    }

    public class PerShardPerkBooks
    {
        [JsonExtensionData]
        public Dictionary<string, JToken> Region { get; set; }
    }

    public class Region
    {
        [JsonProperty("currentPageId")]
        public long CurrentPageId { get; set; }

        [JsonProperty("pages")]
        public Page[] Pages { get; set; }
    }

    public class Settings
    {
        [JsonProperty("gameplayPatchVersionSeen")]
        public string GameplayPatchVersionSeen { get; set; }

        [JsonProperty("gameplayUpdatedPerksSeen")]
        public long[] GameplayUpdatedPerksSeen { get; set; }

        [JsonProperty("gridModeEnabled")]
        public bool GridModeEnabled { get; set; }

        [JsonProperty("showLongDescriptions")]
        public bool ShowLongDescriptions { get; set; }

        [JsonProperty("showPresetPages")]
        public bool ShowPresetPages { get; set; }
    }

    public class Page
    {
        [JsonProperty("autoModifiedSelections")]
        public object[] AutoModifiedSelections { get; set; }

        [JsonProperty("current")]
        public bool Current { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("isDeletable")]
        public bool IsDeletable { get; set; }

        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty("isValid")]
        public bool IsValid { get; set; }

        [JsonProperty("lastModified", NullValueHandling = NullValueHandling.Ignore)]
        public long? LastModified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("primaryStyleId")]
        public long PrimaryStyleId { get; set; }

        [JsonProperty("selectedPerkIds")]
        public long[] SelectedPerkIds { get; set; }

        [JsonProperty("subStyleId")]
        public long SubStyleId { get; set; }

        [JsonProperty("formatVersion", NullValueHandling = NullValueHandling.Ignore)]
        public long? FormatVersion { get; set; }
    }
}
