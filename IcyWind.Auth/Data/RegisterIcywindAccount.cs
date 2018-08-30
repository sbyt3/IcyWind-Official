using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth.Data
{
    public class RegisterIcyWindAccount
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("user")]
        public string Username { get; set; }

        [JsonProperty("pass")]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public string Password
        {
            set => PasswordHash = IcyWindHelpers.Hash(value);
        }
    }
}
