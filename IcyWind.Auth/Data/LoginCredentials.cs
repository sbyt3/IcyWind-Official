using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth.Data
{
    /// <summary>
    /// Represents a user's login Credentials
    /// </summary>
    public class LoginCredentials
    {
        /// <summary>
        /// The user's username
        /// </summary>
        [JsonProperty("user")]
        public string Username { get; set; }

        /// <summary>
        /// The user's password for transmission (Hashed)
        /// </summary>
        [JsonProperty("pass")]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        public string Password
        {
            set => PasswordHash = IcyWindHelpers.Hash(value);
        }
    }
}
