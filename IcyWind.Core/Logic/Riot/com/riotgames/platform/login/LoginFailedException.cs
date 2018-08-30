using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.login
{
    [RtmpSharp("com.riotgames.platform.login.LoginFailedException")]
    public class LoginFailedException : RiotRtmpObject
    {
        [RtmpSharp("message")]
        public object Message { get; set; }

        [RtmpSharp("suppressed")]
        public object[] Suppressed { get; set; }

        [RtmpSharp("rootCauseClassname")]
        public string RootCauseClassname { get; set; }

        [RtmpSharp("localizedMessage")]
        public object LocalizedMessage { get; set; }

        [RtmpSharp("cause")]
        public object Cause { get; set; }

        [RtmpSharp("substitutionArguments")]
        public object SubstitutionArguments { get; set; }

        [RtmpSharp("errorCode")]
        public string ErrorCode { get; set; }

        [RtmpSharp("bannedUntilDate")]
        public double BannedUntilDate { get; set; }
    }
}
