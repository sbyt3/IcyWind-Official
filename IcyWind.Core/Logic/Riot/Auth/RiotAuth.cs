using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DotNetty.Codecs.Compression;
using IcyWind.Auth;
using IcyWind.Auth.Data;
using IcyWind.Core.Logic.Crypt;
using IcyWind.Core.Logic.Data;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.IcyWind.Accounts;
using IcyWind.Core.Logic.Riot.com;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.client.dynamic.configuration;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.login;
using IcyWind.Core.Logic.Riot.Chat;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.Lobby;
using IcyWind.Core.Logic.Riot.RiotData;
using IcyWind.Core.Pages;
using Newtonsoft.Json;
using RtmpSharp;
using RtmpSharp.Net;
using WebSocketSharp;
using YamlDotNet.Serialization;

namespace IcyWind.Core.Logic.Riot.Auth
{
    /// <summary>
    /// Handles authentication to the login servers
    /// A huge pain to gather info on how this is done
    /// </summary>
    /// <remarks>
    /// A lot of the functions have been split up. You should call them in this order
    /// 1. <see cref="GetOpenIdConfig"/>
    /// 2. <see cref="GetLoginToken"/>
    /// 3. <see cref="GetLoginUserInfo"/>
    /// 4. <see cref="GetLoginEntitlements"/>
    /// 5. <see cref="GetLoginLcdsRsoLoginQueue"/>
    /// 6. <see cref="LoginRtmps"/>
    ///
    /// This class also requires the <see cref="RegionData"/> which parses the system.yaml file from riotgames
    /// </remarks>
    public static class RiotAuth
    {
        public static async Task Login(object sender, Dispatcher disp, string user, string pass, string region, RiotAuthOpenIdConfiguration openId, bool save, Action callback)
        {
            var regionData = RiotClientData.ReadSystemRegionData(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml"), region);
            var loginDataAuthToken = GetLoginToken(user, pass, regionData, openId);
            if (loginDataAuthToken.Result != RiotAuthResult.Success)
            {

                await disp.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("InvalidRiotCred").Replace("{User}", user));
                    if (sender.GetType() == typeof(LoginPage))
                    {

                        ((LoginPage)sender).LoginProgressBar.Visibility = Visibility.Hidden;
                    }
                }));
                return;
            }
            var userInfo = GetLoginUserInfo(loginDataAuthToken, openId);
            //I have no idea why this is called, it doesn't seem to be used for much but it simulates the
            //Connections that riot has. Don't question riot, they do funny things.
            var loginEnt = GetLoginEntitlements(regionData, loginDataAuthToken);
            var rsoAuth = GetLoginLcdsRsoLoginQueue(regionData, loginDataAuthToken, userInfo);
            var userClient = await LoginRtmps(regionData, JsonConvert.SerializeObject(rsoAuth.Lqt),
                loginDataAuthToken.AccessTokenJson.AccessToken, user);



            if (userClient.Success)
            {
                userClient.Client.RiotMessagingService = new WebSocket(regionData.Servers.Rms.RmsUrl.Replace(":443","") + 
                                                                       $"/rms/v1/session?token={loginDataAuthToken.AccessTokenJson.AccessToken}&id={Guid.NewGuid():D}&token_type=access");
                userClient.Client.Token = loginDataAuthToken;
                userClient.Client.Region = regionData;
                userClient.Client.OpenId = openId;
                var ping = new Thread(() =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    userClient.Client.RiotMessagingService.Ping();
                });
                userClient.Client.RiotMessagingService.OnMessage +=
                    userClient.Client.OnRiotMessagingServiceReceived;
                userClient.Client.RiotMessagingService.OnOpen += (o, args) => ping.Start();
                //userClient.Client.RiotMessagingService.Connect();

                userClient.Client.SaveToServer = save;

                var str1 = $"gn-{userClient.Client.RiotSession.AccountSummary.AccountId}";
                var str2 = $"cn-{userClient.Client.RiotSession.AccountSummary.AccountId}";
                var str3 = $"bc-{userClient.Client.RiotSession.AccountSummary.AccountId}";
                Task<bool>[] taskArray = { userClient.Client.RiotConnection.SubscribeAsync("my-rtmps", "messagingDestination", str1, str1),
                                userClient.Client.RiotConnection.SubscribeAsync("my-rtmps", "messagingDestination", str2, str2),
                                userClient.Client.RiotConnection.SubscribeAsync("my-rtmps", "messagingDestination", "bc", str3) };

                await Task.WhenAll(taskArray);

                var plainTextbytes = Encoding.UTF8.GetBytes(userClient.Client.RiotSession.AccountSummary.Username + ":" + userClient.Client.RiotSession.Token);
                var reconnectToken = Convert.ToBase64String(plainTextbytes);

                userClient.Client.RiotConnection.MessageReceived += userClient.Client.OnRtmpMessage;

                await userClient.Client.RiotConnection.LoginAsync(user,
                    userClient.Client.RiotSession.Token);

                userClient.Client.RiotProxyCalls = new LcdsRiotCalls((RiotCalls)userClient.Client);

                userClient.Client.RegionData.RegionName = region;

                userClient.Client.LoginDataPacket =
                    await((RiotCalls)userClient.Client).GetLoginDataPacketForUser();

                var pref = GetPlayerPref(loginDataAuthToken,
                    userClient.Client.Configs["PlayerPreferences"]["ServiceEndpoint"].ToString(),
                    regionData.PlatformId.ToString(),
                    userClient.Client.LoginDataPacket.AllSummonerData.Summoner.AcctId.ToString());

                var jsonPref = JsonConvert.DeserializeObject<Dictionary<string, string>>(pref);

                var deserializer = new Deserializer();
                var tempData = Encoding.UTF8.GetString(Deflate.Uncompress(Convert.FromBase64String(jsonPref["data"])));
                TextReader sr = new StringReader(tempData);
                var yamlObject = deserializer.Deserialize(sr);

                var serializer = new JsonSerializer();
                var w = new StringWriter();
                serializer.Serialize(w, yamlObject);

                var jsonAsRunesReforaged = JsonConvert.DeserializeObject<RunesReforaged>(w.ToString());

                userClient.Client.RunesReforaged = jsonAsRunesReforaged;

                //Connect to chat now
                await ChatAuth.ConnectToChat(regionData, userClient.Client, Convert.ToBase64String(Encoding.UTF8.GetBytes(loginDataAuthToken.AccessTokenJson.AccessToken)));

                userClient.Client.DoHeartbeatShit();

                userClient.Client.ActiveBoots =
                    await ((RiotCalls)userClient.Client).GetSummonerActiveBoosts();

                userClient.Client.ChampionList =
                    await ((RiotCalls)userClient.Client).GetAvailableChampions();

                var da = await userClient.Client.RiotProxyCalls.WithMoreThanOneResponce("teambuilder-draft",
                    "retrieveGameloopPlayerInfoV1", 2, "");

                userClient.Client.RiotQueues =
                    await ((RiotCalls) userClient.Client).GetAllQueues();


                //Create a game

                var inventoryToken = GetInventoryFromType(regionData,
                    loginDataAuthToken,
                    userClient.Client.Configs["Inventory"]["BaseServiceUrl"].ToString(),
                    userClient.Client.LoginDataPacket.AllSummonerData.Summoner.Puuid.ToString("D"),
                    userClient.Client.LoginDataPacket.AllSummonerData.Summoner.AcctId.ToString(),
                    "QUEUE_ENTRY");

                var leaguesTierRankToken =
                    await userClient.Client.RiotProxyCalls.DoLcdsProxyCallWithResponse("leagues",
                        "getMySignedQueueTierRank", "");

                var simpleInventoryToken = GetInventorySimpleFromType(regionData,
                    loginDataAuthToken,
                    userClient.Client.Configs["Inventory"]["BaseServiceUrl"].ToString(),
                    userClient.Client.LoginDataPacket.AllSummonerData.Summoner.Puuid.ToString("D"),
                    userClient.Client.LoginDataPacket.AllSummonerData.Summoner.AcctId.ToString(),
                    "CHAMPION&inventoryTypes=CHAMPION_SKIN");

                userClient.Client.InvToken = simpleInventoryToken;

                var summonerToken =
                    await userClient.Client.RiotProxyCalls.DoLcdsProxyCallWithResponse("summoner",
                        "getMySummoner", "");

                //userInfo :D

                var party = new CreatePartyService
                {
                    AccountId = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.AcctId,
                    CreatedAt = 0,
                    CurrentParty = null,
                    EligibilityHash = 0,
                    Parties = null,
                    PlatformId = regionData.PlatformId.ToString(),
                    Puuid = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.Puuid.ToString("D"),
                    Registration = new Registration
                    {
                        InventoryToken = null,
                        InventoryTokens = new[]
                        {
                            JsonConvert.DeserializeObject<InventoryService>(inventoryToken).Data.ItemsJwt,
                        },
                        LeaguesTierRankToken =
                            Encoding.UTF8.GetString(
                                Gzip.Decompress(Convert.FromBase64String(leaguesTierRankToken.Payload))),
                        SimpleInventoryToken = JsonConvert.DeserializeObject<InventoryService>(simpleInventoryToken)
                            .Data
                            .ItemsJwt,
                        SummonerToken = summonerToken.Payload,
                        UserInfoToken = userInfo
                    },
                    ServerUtcMillis = 0,
                    SummonerId = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.SumId,
                    Version = 0
                };

                var sendPartyHelper = new BodyHelper
                {
                    body = JsonConvert.SerializeObject(party),
                    url = $"v1/players/{userClient.Client.LoginDataPacket.AllSummonerData.Summoner.Puuid:D}"
                };

                var partyRtmp = await userClient.Client.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service",
                    "proxy", JsonConvert.SerializeObject(sendPartyHelper));

                userClient.Client.CurrentParty = JsonConvert.DeserializeObject<PartyPayload>(partyRtmp.Payload);

                //New restricted queue method

                sendPartyHelper = new BodyHelper
                {
                    body = "",
                    method = "GET",
                    url = $"v1/parties/{userClient.Client.CurrentParty.Payload.CurrentParty.PartyId}/eligibility"
                };

                var partyElg = await userClient.Client.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service",
                    "proxy", JsonConvert.SerializeObject(sendPartyHelper));


                //Fucking removed by riot
                /*
                //Get restricted Queues
                var activeQueues = userClient.Client.RiotQueues.Where(x => x.QueueState == "ON");

                var str = string.Empty;
                var queueArr = activeQueues.Select(queue => (int)queue.Id).OrderBy(i => i);
                str = queueArr.Aggregate(str, (current, queue) => current + (queue + ","));

                str = str.Remove(str.Length - 1);

                var resp = await userClient.Client.RiotProxyCalls.DoLcdsProxyCallWithResponse("gatekeeper",
                    "getQueueRestrictionsForQueuesV2", $"{{\"queueIds\":[{str}],\"queueRestrictionsToExclude\":" +
                                                       "[\"QUEUE_DODGER\",\"LEAVER_BUSTED\",\"LEAVER_BUSTER" +
                                                       $"_TAINTED_WARNING\"],\"summonerIds\":[{userClient.Client.LoginDataPacket.AllSummonerData.Summoner.SumId}]}}");
                //*/

                if (StaticVars.UserClientList == null)
                {
                    StaticVars.UserClientList = new List<UserClient>();
                }
                StaticVars.UserClientList.Add(userClient.Client);

                if (sender.GetType() == typeof(AccountSelectorPage) && save)
                {
                    var acc = new IcyWindRiotAccountInfo
                    {
                        Password = pass,
                        Region = region,
                        Username = user,
                        SumIcon = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId,
                        SumLevel = userClient.Client.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel,
                        SumName = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.InternalName,
                    };
                    if (StaticVars.AccountInfo == null)
                    {
                        return;
                    }

                    if (StaticVars.AccountInfo.Accounts == null)
                    {
                        StaticVars.AccountInfo.Accounts = string.Empty;
                    }

                    var crypto = AES.EncryptBase64(StaticVars.Password, JsonConvert.SerializeObject(acc));

                    /*
                    if (StaticVars.LoginCred.AccountData.All(x => x.CryptString != crypto))
                    {
                        StaticVars.LoginCred.AccountData.Add(new IcyWindRiotAccountCrypted
                        {
                            CryptString = crypto,
                            Region = region,
                            SumUuid = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.Puuid.ToString(),
                            SumId = userClient.Client.LoginDataPacket.AllSummonerData.Summoner.SumId
                        });
                        if (callback != null)
                        {
                            await disp.BeginInvoke(DispatcherPriority.Render, callback);
                        }
                    }
                    else
                    {
                        return;
                    }

                    if (StaticVars.UserClientList.Count(x => x.SaveToServer) > 4 && !(StaticVars.AccountInfo.IsDev ||
                                                                                      StaticVars.AccountInfo.IsPaid ||
                                                                                      StaticVars.AccountInfo.IsPro))
                    {
                        return;
                    }
                    var output = await AccountManager.UpdateAccountInfo(StaticVars.LoginCred);

                    //*/
                }
            }
        }

        /// <summary>
        /// This retrieves the OpenId Config
        /// </summary>
        /// <param name="url">The url of the OpenId Config</param>
        /// <returns>The config as <seealso cref="RiotAuthOpenIdConfiguration"></seealso></returns>
        public static RiotAuthOpenIdConfiguration GetOpenIdConfig(string url = "https://auth.riotgames.com/.well-known/openid-configuration")
        {
            using (var client = new WebClient())
            {
                return JsonConvert.DeserializeObject<RiotAuthOpenIdConfiguration>(client.DownloadString(url));
            }
        }

        /// <summary>
        /// Retrieve the User Login Token
        /// </summary>
        /// <param name="username">The username of the player</param>
        /// <param name="password">The password of the player</param>
        /// <param name="regionData">The region the player wants to connect to</param>
        /// <param name="config">The OpenId Config from <see cref="GetOpenIdConfig"/></param>
        /// <returns>The login token or failure information as <see cref="RiotAuthToken"/></returns>
        public static RiotAuthToken GetLoginToken(string username, string password, RegionData regionData, RiotAuthOpenIdConfiguration config)
        {
            var dsid = Guid.NewGuid().ToString("N");

            //The information to Post in the webrequest 
            var postString = "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&" +
                             $"client_assertion={regionData.Rso.Token}&" +
                             "grant_type=password&" +
                             $"username={regionData.PlatformId}|{username}&" +
                             $"password={password}&" +
                             "scope=openid offline_access lol ban profile email phone";
            var result = RiotWebClient.Post(config.TokenEndpoint, RiotWebClientUserAgents.RsoAuth, "application/x-www-form-urlencoded", "application/json", postString, ("X-Riot-DSID", dsid));

            if (result.Success)
            {
                return new RiotAuthToken(RiotAuthResult.Success, result.Result, dsid, regionData);
            }

            return result.Result.Contains("invalid_credentials")
                ? new RiotAuthToken(RiotAuthResult.InvalidCredentials, null, dsid, regionData)
                : new RiotAuthToken(RiotAuthResult.UnknownReason, null, dsid, regionData);
        }

        public static RiotAuthToken RefreshLoginToken(RiotAuthOpenIdConfiguration openId, string refreshToken, RegionData regionData)
        {
            var dsid = Guid.NewGuid().ToString("N");

            //The information to Post in the webrequest 
            var postString = "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&" +
                             $"client_assertion={regionData.Rso.Token}&" +
                             "grant_type=refresh_token&" +
                             $"refresh_token={refreshToken}&" +
                             "scope=openid offline_access lol ban profile email phone";
            var result = RiotWebClient.Post(openId.TokenEndpoint, RiotWebClientUserAgents.RsoAuth, "application/x-www-form-urlencoded", "application/json", postString, ("X-Riot-DSID", dsid));

            if (result.Success)
            {
                return new RiotAuthToken(RiotAuthResult.Success, result.Result, dsid, regionData);
            }

            return result.Result.Contains("invalid_credentials")
                ? new RiotAuthToken(RiotAuthResult.InvalidCredentials, null, dsid, regionData)
                : new RiotAuthToken(RiotAuthResult.UnknownReason, null, dsid, regionData);
        }

        //https://playerpreferences.riotgames.com/playerPref/v3/getPreference/213199017/EUW1/PerksPreferences/?hash=098dcbd9e713bc28cb383642f7d3a7f3a3ae181c6b9787e4734b88a13679e4dd
        
        public static string GetPlayerPref(RiotAuthToken token, string url, string region, string sumId)
        {
            return RiotWebClient.GetWithBearer(url.Replace(":443", "") + $"/playerPref/v3/getPreference/{sumId}/{region}/PerksPreferences/", RiotWebClientUserAgents.PlayerPref,
                token.AccessTokenJson.AccessToken, "application/json", "application/json").Result;
        }

        /// <summary>
        /// Gets some user info that is needed for <see cref="GetLoginLcdsRsoLoginQueue"/>
        /// </summary>
        /// <param name="token">The result from <see cref="GetLoginToken"/></param>
        /// <param name="config">The OpenId Config from <see cref="GetOpenIdConfig"/> that is used in <see cref="GetLoginToken"/></param>
        /// <returns>The user info as a string</returns>
        public static string GetLoginUserInfo(RiotAuthToken token, RiotAuthOpenIdConfiguration config)
        {
            return RiotWebClient.GetWithBearer(config.UserinfoEndpoint, RiotWebClientUserAgents.RsoAuth,
                token.AccessTokenJson.AccessToken, "application/x-www-form-urlencoded", "application/json").Result;
        }

        /// <summary>
        /// Retrieves the entitlements that I have no idea what they do
        /// </summary>
        /// <param name="regionData">The region the player wants to connect to used in <see cref="GetLoginToken"/></param>
        /// <param name="token">The result from <see cref="GetLoginToken"/></param>
        /// <returns></returns>
        public static EntitlementsTokenJson GetLoginEntitlements(RegionData regionData, RiotAuthToken token)
        {
            var result = RiotWebClient.PostWithBearer(regionData.Servers.Entitlements.ExternalUrl,
                RiotWebClientUserAgents.Entitlements, token.AccessTokenJson.AccessToken, "application/json",
                "application/json", "{\r\n    \"urn\": \"urn:entitlement:%\"\r\n}");
            return result.Success ? JsonConvert.DeserializeObject<EntitlementsTokenJson>(result.Result) : null;
        }

        public static RiotLcdsRsoLoginCredentials GetLoginLcdsRsoLoginQueue(RegionData regionData, RiotAuthToken token, string userInfo)
        {
            var result = RiotWebClient.PostWithBearer(regionData.Servers.Lcds.LoginQueueUrl + "/authenticate/RSO",
                RiotWebClientUserAgents.Entitlements, token.AccessTokenJson.AccessToken,
                "application/x-www-form-urlencoded", "application/json", $"userinfo={userInfo}");
            if (result.Success)
            {
                var output = JsonConvert.DeserializeObject<RiotLcdsRsoLoginCredentials>(result.Result);
                output.RsoLoginCredentialsString = result.Result;
                return output;
            }
            return null;
        }

        public static string GetInventoryFromType(RegionData regionData, RiotAuthToken token, string baseUrl, string puuid, string accId, string type, string extra = "")
        {
            if (!string.IsNullOrWhiteSpace(extra))
            {
                extra = "&" + extra;
            }
            var result = RiotWebClient.GetWithBearer(baseUrl + $"/lolinventoryservice/v2/inventories?puuid={puuid}&inventoryTypes={type.ToUpper()}&location={regionData.Servers.DiscoverousServiceLocation}&accountId={accId}&signed=true" + extra,
                RiotWebClientUserAgents.Inventory, token.AccessTokenJson.AccessToken,
                "application/json", "application/json");
            return result.Success ? result.Result : null;
        }

        public static string GetInventorySimpleFromType(RegionData regionData, RiotAuthToken token, string baseUrl, string puuid, string accId, string type, string extra = "")
        {
            if (!string.IsNullOrWhiteSpace(extra))
            {
                extra = "&" + extra;
            }
            var result = RiotWebClient.GetWithBearer(baseUrl + $"/lolinventoryservice/v2/inventories/simple?puuid={puuid}&location={regionData.Servers.DiscoverousServiceLocation}&accountId={accId}&inventoryTypes={type}&signed=true" + extra,
                RiotWebClientUserAgents.Inventory, token.AccessTokenJson.AccessToken,
                "application/json", "application/json");
            return result.Success ? result.Result : null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionData"></param>
        /// <param name="rsoAuthToken"></param>
        /// <param name="partnerCred">Same as AccessToken</param>
        /// <returns></returns>
        public static async Task<RtmpLoginResult> LoginRtmps(RegionData regionData, string rsoAuthToken, string partnerCred, string userName)
        {
            var client = new RiotCalls();
            var rtmpClasses = HelperFunctions.GetInstances<RiotRtmpObject>();
            var typeList = new List<Type>();
            foreach (var rtmpClass in rtmpClasses)
            {
                typeList.Add(rtmpClass.GetType());
            }
            var context = new SerializationContext(typeList.ToArray());
            var options = new RtmpClient.Options
            {
                Url = $"rtmps://{regionData.Servers.Lcds.LcdsHost}:{regionData.Servers.Lcds.LcdsPort}",
                Context = context,
                Validate = (sender, certificate, chain, errors) => true,
                AppName = "LCU",
                PageUrl = null,
                SwfUrl = null,
                ChunkLength = 16500
            };

            //Pass to the rtmpclient
            client.RiotConnection = await RtmpClient.ConnectAsync(options);

            client.IsConnectedToRtmp = true;

            var authCred = new AuthenticationCredentials
            {
                MacAddress = LoginSystemData.MacAddress,
                AuthToken = rsoAuthToken,
                PartnerCredentials = partnerCred,
                OperatingSystem = LoginSystemData.OperatingSystem,
                Username = userName
            };

            var loginResult = await client.Login(authCred);
            if (loginResult.GetType() == typeof(Session))
            {
                client.RegionData = regionData;
                client.RiotSession = (Session) loginResult;
                return new RtmpLoginResult(true, client, null);
            }

            if (loginResult.GetType() != typeof(LoginFailedException))
            {
                return new RtmpLoginResult(false, null, null);
            }

            client.IsConnectedToRtmp = false;
            await client.RiotConnection.CloseAsync();
            return new RtmpLoginResult(false, null, (LoginFailedException)loginResult);

        }
    }

    public class RtmpLoginResult
    {
        public RtmpLoginResult(bool success, UserClient client, LoginFailedException ex)
        {
            Success = success;
            Client = client;
            LoginException = ex;
        }
        public bool Success { get; }
        public UserClient Client { get; }
        public LoginFailedException LoginException { get; }
    }

    public class RiotAuthToken
    {
        public RiotAuthToken(RiotAuthResult result, string accessTokenJson, string dsid, RegionData regionData)
        {
            Result = result;
            Dsid = dsid;
            RegionData = regionData;

            if (result == RiotAuthResult.Success)
                AccessTokenJson = JsonConvert.DeserializeObject<AccessTokenJson>(accessTokenJson);
        }

        public RiotAuthResult Result { get; }

        public string Dsid { get; }

        public AccessTokenJson AccessTokenJson { get; }

        public RegionData RegionData { get; }
    }

    public enum RiotAuthResult
    {
        Success,
        InvalidCredentials,
        UnknownReason
    }

    public class AccessTokenJson
    {

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("id_token")]
        public string IdToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }

    public class EntitlementsTokenJson
    {
        [JsonProperty("entitlements_token")]
        public string EntitlementsToken { get; set; }
    }
}
