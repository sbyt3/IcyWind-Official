using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth.Data
{
    public class LoginCredentials
    {
        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("pass")]
        public string PasswordHash { get; set; }

        [JsonProperty("data")]
        public string Data { get; internal set; }

        [JsonIgnore]
        public List<IcyWindRiotAccountCrypted> AccountData { get; set; }



        [JsonIgnore]
        public string Password
        {
            set => PasswordHash = IcyWindHelpers.Hash(value);
        }
    }
}
