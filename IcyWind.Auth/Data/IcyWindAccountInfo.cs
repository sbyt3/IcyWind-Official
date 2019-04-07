using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IcyWind.Auth.Data
{
    public class IcyWindAccountInfo
    {
        /// <summary>
        /// User's name defined from IcyWind.gg
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user's Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A bool representing if a user has verified their email
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// The accounts the user has (Must decode)
        /// </summary>
        public IcyWindRiotAccountCrypted Accounts { get; set; }

        /// <summary>
        /// The rank of the user
        /// </summary>
        public string IcywindRank { get; set; }

        /// <summary>
        /// Represents if a user is banned
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// Represents if a user is a dev
        /// </summary>
        public bool IsDev { get; set; }

        /// <summary>
        /// Represents if a user has paid for premium features
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Represents if a user is a pro player
        /// </summary>
        public bool IsPro { get; set; }

        /// <summary>
        /// Represents how much the user pays monthly
        /// </summary>
        public long PaidAmount { get; set; }

        /// <summary>
        /// A random account UUID generated upon signup
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// Current accessing IP Address. Used for 2FA
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Represents an error on login
        /// </summary>
        [JsonProperty("err")]
        public string Error { get; set; }

        /// <summary>
        /// Represents two factor auth
        /// </summary>
        [JsonProperty("2fa")]
        public string TwoFactor { get; set; }
    }
}
