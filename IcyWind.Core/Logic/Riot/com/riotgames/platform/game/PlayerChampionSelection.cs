using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{
    [RtmpSharp("com.riotgames.platform.game.PlayerChampionSelectionDTO")]
    public partial class PlayerChampionSelection : RiotRtmpObject 
    {
        [RtmpSharp("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("spell2Id")]
        public long Spell2Id { get; set; }

        [RtmpSharp("selectedSkinIndex")]
        public long SelectedSkinIndex { get; set; }

        [RtmpSharp("championId")]
        public long ChampionId { get; set; }

        [RtmpSharp("spell1Id")]
        public long Spell1Id { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
