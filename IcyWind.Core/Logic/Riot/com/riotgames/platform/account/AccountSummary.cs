using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.account
{
    [RtmpSharp("com.riotgames.platform.account.AccountSummary")]
    public class AccountSummary : RiotRtmpObject
    {
        [RtmpSharp("groupCount")]
        public int? GroupCount { get; set; }

        [RtmpSharp("username")]
        public string Username { get; set; }

        [RtmpSharp("accountId")]
        public long AccountId { get; set; }

        [RtmpSharp("summonerInternalName")]
        public object SummonerInternalName { get; set; }

        [RtmpSharp("dataVersion")]
        public int? DataVersion { get; set; }

        [RtmpSharp("admin")]
        public bool Admin { get; set; }

        [RtmpSharp("hasBetaAccess")]
        public bool HasBetaAccess { get; set; }

        [RtmpSharp("summonerName")]
        public object SummonerName { get; set; }

        [RtmpSharp("partnerMode")]
        public bool PartnerMode { get; set; }

        [RtmpSharp("needsPasswordReset")]
        public bool NeedsPasswordReset { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }
    }
}
