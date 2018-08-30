using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{
    [RtmpSharp("com.riotgames.platform.game.GameDTO")]
    public partial class GameDto : RiotRtmpObject
    {
        [RtmpSharp("spectatorsAllowed")]
        public string SpectatorsAllowed { get; set; }

        [RtmpSharp("passwordSet")]
        public bool PasswordSet { get; set; }

        [RtmpSharp("practiceGameRewardsDisabledReasons")]
        public object[] PracticeGameRewardsDisabledReasons { get; set; }

        [RtmpSharp("gameType")]
        public string GameType { get; set; }

        [RtmpSharp("gameTypeConfigId")]
        public long GameTypeConfigId { get; set; }

        [RtmpSharp("gameState")]
        public string GameState { get; set; }

        [RtmpSharp("observers")]
        public object[] Observers { get; set; }

        [RtmpSharp("statusOfParticipants")]
        public object StatusOfParticipants { get; set; }

        [RtmpSharp("id")]
        public long Id { get; set; }

        [RtmpSharp("ownerSummary")]
        public object OwnerSummary { get; set; }

        [RtmpSharp("teamTwoPickOutcome")]
        public object TeamTwoPickOutcome { get; set; }

        [RtmpSharp("teamTwo")]
        public PlayerParticipant[] TeamTwo { get; set; }

        [RtmpSharp("bannedChampions")]
        public object[] BannedChampions { get; set; }

        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("roomName")]
        public object RoomName { get; set; }

        [RtmpSharp("name")]
        public string Name { get; set; }

        [RtmpSharp("spectatorDelay")]
        public long SpectatorDelay { get; set; }

        [RtmpSharp("teamOne")]
        public PlayerParticipant[] TeamOne { get; set; }

        [RtmpSharp("terminatedCondition")]
        public string TerminatedCondition { get; set; }

        [RtmpSharp("queueTypeName")]
        public string QueueTypeName { get; set; }

        [RtmpSharp("featuredGameInfo")]
        public FeaturedGameInfo FeaturedGameInfo { get; set; }

        [RtmpSharp("passbackUrl")]
        public object PassbackUrl { get; set; }

        [RtmpSharp("roomPassword")]
        public string RoomPassword { get; set; }

        [RtmpSharp("optimisticLock")]
        public long OptimisticLock { get; set; }

        [RtmpSharp("teamOnePickOutcome")]
        public object TeamOnePickOutcome { get; set; }

        [RtmpSharp("maxNumPlayers")]
        public long MaxNumPlayers { get; set; }

        [RtmpSharp("queuePosition")]
        public long QueuePosition { get; set; }

        [RtmpSharp("terminatedConditionString")]
        public string TerminatedConditionString { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("gameMode")]
        public string GameMode { get; set; }

        [RtmpSharp("expiryTime")]
        public long ExpiryTime { get; set; }

        [RtmpSharp("mapId")]
        public long MapId { get; set; }

        [RtmpSharp("banOrder")]
        public object[] BanOrder { get; set; }

        [RtmpSharp("gameStateString")]
        public string GameStateString { get; set; }

        [RtmpSharp("pickTurn")]
        public long PickTurn { get; set; }

        [RtmpSharp("playerChampionSelections")]
        public PlayerChampionSelection[] PlayerChampionSelections { get; set; }

        [RtmpSharp("gameMutators")]
        public string[] GameMutators { get; set; }

        [RtmpSharp("joinTimerDuration")]
        public long JoinTimerDuration { get; set; }

        [RtmpSharp("passbackDataPacket")]
        public object PassbackDataPacket { get; set; }
    }

}
