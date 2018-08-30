using RtmpSharp;

//Fucking pain in the ass garbage int name
namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.login
{
    /// <summary>
    ///     This class handles the authentication into the server
    /// </summary>
    [RtmpSharp("com.riotgames.platform.login.AuthenticationCredentials")]
    public class AuthenticationCredentials : RiotRtmpObject
    {
        [RtmpSharp("macAddress")] public string MacAddress { get; set; }

        [RtmpSharp("authToken")] public string AuthToken { get; set; }

        [RtmpSharp("domain")] public string Domain { get; set; } = "lolclient.lol.riotgames.com";

        [RtmpSharp("securityAnswer")] public string SecurityAnswer { get; set; } = null;

        [RtmpSharp("oldPassword")] public string OldPassword { get; set; } = null;

        [RtmpSharp("clientVersion")] public string ClientVersion { get; set; } = "LCU";

        [RtmpSharp("locale")] public string Locale { get; set; } = "en_US";

        [RtmpSharp("username")] public string Username { get; set; }

        [RtmpSharp("partnerCredentials")] public string PartnerCredentials { get; set; }

        [RtmpSharp("operatingSystem")] public string OperatingSystem { get; set; } = "Windows 7";

        [RtmpSharp("password")] public string Password { get; set; } = null;
    }
}