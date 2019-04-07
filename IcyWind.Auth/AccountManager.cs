using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Auth.Data;
using Newtonsoft.Json;

namespace IcyWind.Auth
{
    public static class AccountManager
    {
        private static readonly HttpClient client = new HttpClient();

        public static string RegisterAccount(RegisterIcyWindAccount details)
        {
            return "{\"error\":\"Registration is not supported in IcyWind Client. Please go to icywindclient.gg\"}";
        }

    }
}
