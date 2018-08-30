using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner
{
    [RtmpSharp("com.riotgames.platform.summoner.Summoner")]
    public class Summoner : RiotRtmpObject
    {
        [RtmpSharp("internalName")]
        public string InternalName { get; set; }

        [RtmpSharp("previousSeasonHighestTier")]
        public string PreviousSeasonHighestTier { get; set; }

        [RtmpSharp("acctId")]
        public long AcctId { get; set; }

        [RtmpSharp("helpFlag")]
        public bool HelpFlag { get; set; }

        [RtmpSharp("sumId")]
        public long SumId { get; set; }

        [RtmpSharp("profileIconId")]
        public int ProfileIconId { get; set; }

        [RtmpSharp("displayEloQuestionaire")]
        public bool DisplayEloQuestionaire { get; set; }

        [RtmpSharp("lastGameDate")]
        public string LastGameDate { get; set; }

        [RtmpSharp("previousSeasonHighestTeamReward")]
        public int PreviousSeasonHighestTeamReward { get; set; }

        [RtmpSharp("revisionDate")]
        public string RevisionDate { get; set; }

        [RtmpSharp("advancedTutorialFlag")]
        public bool AdvancedTutorialFlag { get; set; }

        [RtmpSharp("revisionId")]
        public int RevisionId { get; set; }

        [RtmpSharp("puuid")]
        public Guid Puuid { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("nameChangeFlag")]
        public bool NameChangeFlag { get; set; }

        [RtmpSharp("tutorialFlag")]
        public bool TutorialFlag { get; set; }
    }
}
