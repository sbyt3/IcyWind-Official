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

            var values = new Dictionary<string, string>
            {
                { "registerInfo", IcyWindHelpers.ConvertToBase64Html(details) }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("https://api.icywindclient.com/v1/login/RegisterAccount", content).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        public static IcyWindAccountInfo GetAccountInfo(LoginCredentials loginCredentials)
        {
            var values = new Dictionary<string, string>
            {
                { "loginInfo", IcyWindHelpers.ConvertToBase64Html(loginCredentials) }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("https://api.icywindclient.com/v1/login/GetAccountInfo", content).Result;

            return JsonConvert.DeserializeObject<IcyWindAccountInfo>(response.Content.ReadAsStringAsync().Result);
        }

        public static async Task<string> UpdateAccountInfo(LoginCredentials loginCredentials)
        {
            List<string> output = new List<string>();
            foreach (var converter in loginCredentials.AccountData)
            {
                output.Add(IcyWindHelpers.ConvertToBase64Html(converter));
            }
            loginCredentials.Data = JsonConvert.SerializeObject(output);

            var values = new Dictionary<string, string>
            {
                { "loginInfo", IcyWindHelpers.ConvertToBase64Html(loginCredentials) }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.icywindclient.com/v1/login/UpdateAccountInfo", content);

            return await response.Content.ReadAsStringAsync();
        }

        public static string Enable2FA(LoginCredentials loginCredentials)
        {
            var values = new Dictionary<string, string>
            {
                { "loginInfo", IcyWindHelpers.ConvertToBase64Html(loginCredentials) }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("https://api.icywindclient.com/v1/login/Enable2FA", content).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
