using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Auth.Data;
using log4net;
using Microsoft.Extensions.Logging;

namespace IcyWind.Core.Logic.IcyWind
{
    public static class StaticVars
    {
        public static string LeagueLocation { get; set; }

        public static string IcyWindLocation { get; set; }

        internal static LoginCredentials LoginCred { get; set; }

        public static UserClient ActiveClient { get; set; }

        internal static string Password { get; set; }

        internal static IcyWindAccountInfo AccountInfo { get; set; }

        public static ILog Logger { get; set; }

        public static List<UserClient> UserClientList = new List<UserClient>();
    }
}
