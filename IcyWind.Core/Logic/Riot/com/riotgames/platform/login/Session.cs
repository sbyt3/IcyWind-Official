using IcyWind.Core.Logic.Riot.com.riotgames.platform.account;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.login
{
    [RtmpSharp("com.riotgames.platform.login.Session")]
    public class Session : RiotRtmpObject
    {
        [RtmpSharp("token")]
        public string Token { get; set; }

        /// <summary>
        ///     Dumb. Output is pass{Last 5 dig of accountId}
        /// </summary>
        [RtmpSharp("password")]
        public string Password { get; set; }

        [RtmpSharp("accountSummary")]
        public AccountSummary AccountSummary { get; set; }
    }
}