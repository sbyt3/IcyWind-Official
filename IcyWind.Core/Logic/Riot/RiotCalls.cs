using System;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.com;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.champion;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.clientfacade.domain;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.login;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.boost;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.RiotData;
using Newtonsoft.Json;
using RtmpSharp;
using RtmpSharp.Messaging;

namespace IcyWind.Core.Logic.Riot
{
    /// <summary>
    /// This inherates the UserClient class, because they actually should be together, but there is just too many calls in here that it is better to just inherate it
    /// </summary>
    public class RiotCalls : UserClient
    {
        public delegate void OnInvocationErrorHandler(object sender, Exception error);

        public event OnInvocationErrorHandler OnInvocationError;

        /// <summary>
        ///     Login to Riot's servers.
        /// </summary>
        /// <param name="credentials">The credentials for the user</param>
        /// <returns>Session information for the user</returns>
        public Task<RiotRtmpObject> Login(AuthenticationCredentials credentials)
        {
            return InvokeAsync<RiotRtmpObject>("loginService", "login", credentials);
        }

        public Task<LoginDataPacket> GetLoginDataPacketForUser()
        {
            return InvokeAsync<LoginDataPacket>("clientFacadeService", "getLoginDataPacketForUser");
        }

        /// <summary>
        /// Logs the user out of Riot's servers.
        /// Called before closing or on logout
        /// </summary>
        /// <param name="session">Session returned from <see cref="Login"/></param>
        /// <returns>null</returns>
        public Task<object> Logout(Session session)
        {
            return InvokeAsync<object>("loginService", "logout", session.Token);
        }
        
        public Task<SummonerActiveBoostsDTO> GetSummonerActiveBoosts()
        {
            return InvokeAsync<SummonerActiveBoostsDTO>("inventoryService", "getSummonerActiveBoosts");
        }

        public Task<ChampionDTO[]> GetAvailableChampions()
        {
            return InvokeAsync<ChampionDTO[]>("inventoryService", "getAvailableChampions");
        }

        public async Task<RiotQueue[]> GetAllQueues()
        {
            var str = await InvokeAsync<string>("matchmakerService", "getAllQueuesCompressed");
            return JsonConvert.DeserializeObject<RiotQueue[]>(Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String(str))));
        }

        public Task<string[]> GetSummonerNames(double[] sumId)
        {
            return InvokeAsync<string[]>("summonerService", "getSummonerNames", sumId);
        }

        public Task<string> PerformLcdsHeartBeat(long sumId, string token, int heartbeatCount, string time)
        {
            return InvokeAsync<string>("loginService", "performLCDSHeartBeat", sumId, token, heartbeatCount, time);
        }

        public Task<AllPublicSummonerDataDTO> GetAllPublicSummonerDataByName(string name)
        {
            return InvokeAsync<AllPublicSummonerDataDTO>("summonerService", "getAllPublicSummonerDataByName", name);
        }

        /// <summary>
        ///     Used to send a command to the League of Legends server
        /// </summary>
        /// <typeparam name="T">The exptected return</typeparam>
        /// <param name="destination">The destination of the call</param>
        /// <param name="method">The method of the call</param>
        /// <param name="arguments">The params of the call</param>
        /// <returns>T</returns>
        internal Task<T> InvokeAsync<T>(string destination, string method, params object[] arguments)
        {
            while (!IsConnectedToRtmp)
            {
                Task.Delay(1000);
            }

            try
            {
                return RiotConnection.InvokeAsync<T>("my-rtmps", destination, method, arguments);
            }
            catch (InvocationException e)
            {
                OnInvocationError?.Invoke(null, e);
                return null;
            }
        }
    }
}
