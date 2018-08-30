using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Lobby
{
    public class PartyPhaseMessage
    {
        [JsonProperty("counter")]
        public long Counter { get; set; }

        [JsonProperty("phaseName")]
        public string PhaseName { get; set; }

        [JsonProperty("queueId")]
        public long QueueId { get; set; }

        [JsonProperty("premadeState")]
        public PremadeState PremadeState { get; set; }

        [JsonProperty("matchmakingState")]
        public MatchmakingState MatchmakingState { get; set; }

        [JsonProperty("championSelectState")]
        public ChampionSelectState ChampionSelectState { get; set; }

        [JsonProperty("useInventoryPath2018")]
        public bool UseInventoryPath2018 { get; set; }
    }

    public class ChampionSelectState
    {
        [JsonProperty("teamId")]
        public Guid TeamId { get; set; }

        [JsonProperty("teamChatRoomId")]
        public string TeamChatRoomId { get; set; }

        [JsonProperty("subphase")]
        public string Subphase { get; set; }

        [JsonProperty("actionSetList")]
        public ActionSetList[][] ActionSetList { get; set; }

        [JsonProperty("currentActionSetIndex")]
        public long CurrentActionSetIndex { get; set; }

        [JsonProperty("ceremoniesByActionSetIndex")]
        public CeremoniesByActionSetIndex CeremoniesByActionSetIndex { get; set; }

        [JsonProperty("cells")]
        public Cells Cells { get; set; }

        [JsonProperty("localPlayerCellId")]
        public long LocalPlayerCellId { get; set; }

        [JsonProperty("currentTotalTimeMillis")]
        public long CurrentTotalTimeMillis { get; set; }

        [JsonProperty("currentTimeRemainingMillis")]
        public long CurrentTimeRemainingMillis { get; set; }

        [JsonProperty("trades")]
        public object[] Trades { get; set; }

        [JsonProperty("allowOptingOutOfBanning")]
        public bool AllowOptingOutOfBanning { get; set; }

        [JsonProperty("allowSkinSelection")]
        public bool AllowSkinSelection { get; set; }

        [JsonProperty("allowDuplicatePicks")]
        public bool AllowDuplicatePicks { get; set; }

        [JsonProperty("rerollState")]
        public RerollState RerollState { get; set; }

        [JsonProperty("battleBoostState")]
        public BattleBoostState BattleBoostState { get; set; }

        [JsonProperty("championBenchState")]
        public ChampionBenchState ChampionBenchState { get; set; }
    }

    public class ActionSetList
    {
        [JsonProperty("actionId")]
        public long ActionId { get; set; }

        [JsonProperty("actorCellId")]
        public long ActorCellId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("championId")]
        public long ChampionId { get; set; }

        [JsonProperty("completed")]
        public bool Completed { get; set; }
    }

    public class BattleBoostState
    {
        [JsonProperty("allowBattleBoost")]
        public bool AllowBattleBoost { get; set; }

        [JsonProperty("boostableSkinCount")]
        public long BoostableSkinCount { get; set; }

        [JsonProperty("battleBoostActivated")]
        public bool BattleBoostActivated { get; set; }

        [JsonProperty("activatorCellId")]
        public long ActivatorCellId { get; set; }

        [JsonProperty("unlockedSkinIds")]
        public object[] UnlockedSkinIds { get; set; }

        [JsonProperty("cost")]
        public long Cost { get; set; }
    }

    public class Cells
    {
        [JsonProperty("alliedTeam")]
        public AlliedTeam[] AlliedTeam { get; set; }

        [JsonProperty("enemyTeam")]
        public EnemyTeam[] EnemyTeam { get; set; }
    }

    public class AlliedTeam
    {
        [JsonProperty("teamId")]
        public long TeamId { get; set; }

        [JsonProperty("cellId")]
        public long CellId { get; set; }

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        [JsonProperty("championPickIntent")]
        public long ChampionPickIntent { get; set; }

        [JsonProperty("championId")]
        public long ChampionId { get; set; }

        [JsonProperty("assignedPosition")]
        public string AssignedPosition { get; set; }

        [JsonProperty("spell1Id")]
        public long Spell1Id { get; set; }

        [JsonProperty("spell2Id")]
        public long Spell2Id { get; set; }

        [JsonProperty("skinId")]
        public long SkinId { get; set; }
    }

    public class EnemyTeam
    {
        [JsonProperty("teamId")]
        public long TeamId { get; set; }

        [JsonProperty("cellId")]
        public long CellId { get; set; }

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }
    }

    public class CeremoniesByActionSetIndex
    {
    }

    public class ChampionBenchState
    {
        [JsonProperty("benchEnabled")]
        public bool BenchEnabled { get; set; }

        [JsonProperty("championIds")]
        public object[] ChampionIds { get; set; }
    }

    public class RerollState
    {
        [JsonProperty("allowRerolling")]
        public bool AllowRerolling { get; set; }

        [JsonProperty("rerollsRemaining")]
        public long RerollsRemaining { get; set; }
    }

    public class MatchmakingState
    {
        [JsonProperty("estimatedMatchmakingTimeMillis")]
        public long EstimatedMatchmakingTimeMillis { get; set; }

        [JsonProperty("timeInMatchmakingMillis")]
        public long TimeInMatchmakingMillis { get; set; }
    }

    public class PremadeState
    {
        [JsonProperty("timer")]
        public long Timer { get; set; }

        [JsonProperty("draftPremadeId")]
        public Guid DraftPremadeId { get; set; }

        [JsonProperty("premadeChatRoomId")]
        public Guid PremadeChatRoomId { get; set; }

        [JsonProperty("captainSlotId")]
        public long CaptainSlotId { get; set; }

        [JsonProperty("showPositionSelector")]
        public bool ShowPositionSelector { get; set; }

        [JsonProperty("readyToMatchmake")]
        public bool ReadyToMatchmake { get; set; }

        [JsonProperty("readyState")]
        public ReadyState ReadyState { get; set; }

        [JsonProperty("draftSlots")]
        public DraftSlot[] DraftSlots { get; set; }

        [JsonProperty("playableDraftPositions")]
        public string[] PlayableDraftPositions { get; set; }

        [JsonProperty("localPlayerSlotId")]
        public long LocalPlayerSlotId { get; set; }

        [JsonProperty("autoFillEligible")]
        public bool AutoFillEligible { get; set; }

        [JsonProperty("autoFillProtectedForStreaking")]
        public bool AutoFillProtectedForStreaking { get; set; }

        [JsonProperty("autoFillProtectedForPromos")]
        public bool AutoFillProtectedForPromos { get; set; }
    }

    public class DraftSlot
    {
        [JsonProperty("slotId")]
        public long SlotId { get; set; }

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        [JsonProperty("summonerId")]
        public long SummonerId { get; set; }

        [JsonProperty("draftPositionPreferences")]
        public string[] DraftPositionPreferences { get; set; }

        [JsonProperty("autoFillEligible")]
        public bool AutoFillEligible { get; set; }

        [JsonProperty("autoFillProtectedForStreaking")]
        public bool AutoFillProtectedForStreaking { get; set; }

        [JsonProperty("autoFillProtectedForPromos")]
        public bool AutoFillProtectedForPromos { get; set; }

        [JsonProperty("autoFillProtectedForSoloing")]
        public bool AutoFillProtectedForSoloing { get; set; }
    }

    public partial class ReadyState
    {
        [JsonProperty("readyToMatchmake")]
        public bool ReadyToMatchmake { get; set; }

        [JsonProperty("premadeSizeAllowed")]
        public bool PremadeSizeAllowed { get; set; }

        [JsonProperty("requiredPositionCoverageMet")]
        public bool RequiredPositionCoverageMet { get; set; }

        [JsonProperty("allowablePremadeSizes")]
        public long[] AllowablePremadeSizes { get; set; }
    }
}
