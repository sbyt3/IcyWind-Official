using System.Numerics;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.Riot.Auth
{
    public class RiotLcdsRsoLoginCredentials
    {
        [JsonIgnore]
        public string RsoLoginCredentialsString { get; set; }

        [JsonProperty("rate")]
        public int? Rate { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lqt")]
        public Lqt Lqt { get; set; }

        [JsonProperty("delay")]
        public int? Delay { get; set; }

        [JsonProperty("inGameCredentials")]
        public InGameCredentials InGameCredentials { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }
    }

    public class InGameCredentials
    {
        [JsonProperty("inGame", NullValueHandling = NullValueHandling.Ignore)]
        public bool InGame { get; set; }

        [JsonProperty("summonerId", NullValueHandling = NullValueHandling.Ignore)]
        public long? SummonerId { get; set; }

        [JsonProperty("serverIp", NullValueHandling = NullValueHandling.Ignore)]
        public string ServerIp { get; set; }

        [JsonProperty("serverPort", NullValueHandling = NullValueHandling.Ignore)]
        public int? ServerPort { get; set; }

        [JsonProperty("encryptionKey", NullValueHandling = NullValueHandling.Ignore)]
        public string EncryptionKey { get; set; }

        [JsonProperty("handshakeToken", NullValueHandling = NullValueHandling.Ignore)]
        public object HandshakeToken { get; set; }
    }

    public class Lqt
    {
        [JsonProperty("account_id")]
        public long? AccountId { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("other")]
        public string Other { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("timestamp")]
        public long? Timestamp { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("partner_token")]
        public string PartnerToken { get; set; }

        [JsonProperty("userinfo")]
        public string Userinfo { get; set; }

        [JsonProperty("resources")]
        public string Resources { get; set; }
    }
}
