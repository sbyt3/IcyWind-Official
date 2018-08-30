using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{
    [RtmpSharp("com.riotgames.platform.game.PlayerParticipant")]
    public class PlayerParticipant : RiotRtmpObject
    {
        [RtmpSharp("accountId")]
        public long AccountId { get; set; }

        [RtmpSharp("queueRating")]
        public long QueueRating { get; set; }

        [RtmpSharp("botDifficulty")]
        public string BotDifficulty { get; set; }

        [RtmpSharp("minor")]
        public bool Minor { get; set; }

        [RtmpSharp("locale")]
        public object Locale { get; set; }

        [RtmpSharp("lastSelectedSkinIndex")]
        public long LastSelectedSkinIndex { get; set; }

        [RtmpSharp("gameCustomization")]
        public object GameCustomization { get; set; }

        [RtmpSharp("partnerId")]
        public string PartnerId { get; set; }

        [RtmpSharp("profileIconId")]
        public long ProfileIconId { get; set; }

        [RtmpSharp("timeGameCreated")]
        public long TimeGameCreated { get; set; }

        [RtmpSharp("rankedTeamGuest")]
        public bool RankedTeamGuest { get; set; }

        [RtmpSharp("puuid")]
        public Guid Puuid { get; set; }

        [RtmpSharp("summonerId")]
        public long SummonerId { get; set; }

        [RtmpSharp("voterRating")]
        public long VoterRating { get; set; }

        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("selectedRole")]
        public object SelectedRole { get; set; }

        [RtmpSharp("pickMode")]
        public long PickMode { get; set; }

        [RtmpSharp("teamParticipantId")]
        public long TeamParticipantId { get; set; }

        [RtmpSharp("timeMatchmakingStart")]
        public long TimeMatchmakingStart { get; set; }

        [RtmpSharp("timeAddedToQueue")]
        public long TimeAddedToQueue { get; set; }

        [RtmpSharp("index")]
        public long Index { get; set; }

        [RtmpSharp("timeChampionSelectStart")]
        public long TimeChampionSelectStart { get; set; }

        [RtmpSharp("originalAccountNumber")]
        public long OriginalAccountNumber { get; set; }

        [RtmpSharp("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [RtmpSharp("adjustmentFlags")]
        public long AdjustmentFlags { get; set; }

        [RtmpSharp("teamOwner")]
        public bool TeamOwner { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("teamRating")]
        public long TeamRating { get; set; }

        [RtmpSharp("pickTurn")]
        public long PickTurn { get; set; }

        [RtmpSharp("clientInSynch")]
        public bool ClientInSynch { get; set; }

        [RtmpSharp("summonerName")]
        public string SummonerName { get; set; }

        [RtmpSharp("originalPlatformId")]
        public object OriginalPlatformId { get; set; }

        [RtmpSharp("selectedPosition")]
        public string SelectedPosition { get; set; }
    }




    public class GameCustomization
    {
        [RtmpSharp("summonerEmotes")]
        public string SummonerEmotes { get; set; }

        [RtmpSharp("perks")]
        public string Perks { get; set; }
    }
}
