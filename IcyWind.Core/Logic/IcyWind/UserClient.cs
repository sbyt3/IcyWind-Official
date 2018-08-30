using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Xml;
using DotNetty.Transport.Channels;
using IcyWind.Chat;
using IcyWind.Core.Logic.Data;
using IcyWind.Core.Logic.Riot;
using IcyWind.Core.Logic.Riot.Auth;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.catalog.champion;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.client.dynamic.configuration;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.clientfacade.domain;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.game;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.login;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.summoner.boost;
using IcyWind.Core.Logic.Riot.Chat;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.Lobby;
using IcyWind.Core.Logic.Riot.RiotData;
using IcyWind.Core.Logic.Riot.Rms;
using Newtonsoft.Json;
using RtmpSharp.Messaging;
using RtmpSharp.Net;
using WebSocketSharp;
using String = System.String;

namespace IcyWind.Core.Logic.IcyWind
{
    public class UserClient
    {
        public bool SaveToServer { get; set; }

        public string InvToken { get; set; }

        public bool IsAway { get; set; }

        public bool HasLoggedOut { get; set; }

        public bool IsConnectedToRtmp { get; set; }

        public string Username { get; set; }

        public RtmpClient RiotConnection { get; set; }

        public Session RiotSession { get; set; }

        public LoginDataPacket LoginDataPacket { get; set; }

        public RegionData RegionData { get; set; }

        public ChatClient XmppClient { get; set; }

        public bool IsInGame { get; set; }

        public bool IsInChampSelect { get; set; }

        public bool IsInQueue { get; set; }

        public IChannel ChatConnectionChannel { get; set; }

        public LcdsRiotCalls RiotProxyCalls { get; set; }

        public Timer HeartbeatTimer { get; set; }

        private int HeartbeatCount { get; set; }

        public SummonerActiveBoostsDTO ActiveBoots { get; set; }

        public ChampionDTO[] ChampionList { get; set; }

        public RiotQueue[] RiotQueues { get; set; }

        internal List<ChatPlayerItem> Players { get; set; }

        internal Dictionary<string, Dictionary<string, object>> Configs { get; set; }

        internal WebSocket RiotMessagingService { get; set; }

        internal bool HasEnabledRmsGzip { get; set; } = false;

        internal PartyPayload CurrentParty { get; set; }

        internal string InstaCall { get; set; }

        internal int InstaChamp { get; set; }

        internal RiotAuthToken Token { get; set; }

        internal RegionData Region { get; set; }

        internal RiotAuthOpenIdConfiguration OpenId { get; set; }

        internal RunesReforaged RunesReforaged { get; set; }


        internal delegate void UpdatePlayerHandler(object sender, ChatPlayerItem e);
        internal event UpdatePlayerHandler OnUpdatePlayer;

        internal static string GameDirectory { get; set; } = GetGameDirectory();
        internal static string RootLocation { get; set; }


        public RiotCalls GetRiotCalls()
        {
            return (RiotCalls) this;
        }

        public void OnRiotMessagingServiceReceived(object sender, MessageEventArgs message)
        {
            if (!HasEnabledRmsGzip)
            {
                var data = JsonConvert.DeserializeObject<RiotMessageService>(Encoding.Default.GetString(message.RawData));
                if (data.Subject == "rms:session")
                {
                    RiotMessagingService.Send(Encoding.UTF8.GetBytes("{\r\n    " +
                                              $"\"id\": \"{Guid.NewGuid():D}\",\r\n    " +
                                              "\"payload\": " +
                                              "{\r\n        " +
                                              "\"enable\": \"true\"\r\n    " +
                                              "},\r\n    " +
                                              "\"subject\": \"rms:gzip\",\r\n    " +
                                              "\"type\": \"request\"\r\n}"));
                }
                else if (data.Subject == "rms:gzip" && data.Payload.Enabled == "true")
                {
                    HasEnabledRmsGzip = true;
                }
                Debugger.Log(0, "", Encoding.Default.GetString(message.RawData) + "\n");
            }
            else
            {
                var data = Encoding.UTF8.GetString(Gzip.Decompress(message.RawData));
                Debugger.Log(0, "", data);
            }
        }

        internal static string GetGameDirectory()
        {
            RootLocation = RiotClientData.GetLolRootPath();
            string Directory = Path.Combine(RootLocation, "RADS", "solutions", "lol_game_client_sln", "releases");

            var dInfo = new DirectoryInfo(Directory);
            DirectoryInfo[] subdirs;
            try
            {
                subdirs = dInfo.GetDirectories();
            }
            catch
            {
                return "0.0.0";
            }
            string latestVersion = "0.0.1";
            latestVersion = subdirs.Last().Name;

            Directory = Path.Combine(Directory, latestVersion, "deploy");

            return Directory;
        }

        public void OnRtmpMessage(object sender, MessageReceivedEventArgs eventArgs)
        {
            if (eventArgs.Body is ClientDynamicConfigurationNotification notif)
            {
                Configs = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(
                    Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String(notif.Configs))));
                
            }
            else if (eventArgs.Body is PlayerCredentialsDTO cred)
            {
                StartGame(cred);
            }
            else if (eventArgs.Body is EndOfGameStats stats)
            {
                UserInterfaceCore.ChangeMainPageView<EndOfGameStats>(stats);
            }
        }

        private void StartGame(PlayerCredentialsDTO cred)
        {
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = GameDirectory,
                    FileName = Path.Combine(GameDirectory, "League of Legends.exe")
                }
            };
            p.Exited += p_Exited;
            p.StartInfo.Arguments = "\"8394\" \"" + RootLocation + "LoLLauncher.exe" + "\" \"" + "\" \"" +
                                    cred.ServerIp + " " +
                                    cred.ServerPort + " " +
                                    cred.EncryptionKey + " " +
                                    cred.SummonerId + "\"";
            p.Start();
        }

        private static void p_Exited(object sender, EventArgs e)
        {
            //TODO: Handle this to change the chat
        }

        public void DoHeartbeatShit()
        {
            HeartbeatTimer = new Timer();
            HeartbeatTimer.Elapsed += DoDumbHeartbeatShitElapsed;
            HeartbeatCount = 1;
            HeartbeatTimer.Interval = 120000; // in milliseconds
            HeartbeatTimer.Start();
        }

        private async void DoDumbHeartbeatShitElapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsConnectedToRtmp)
                return;
            //Refresh token
            Token = RiotAuth.GetLoginToken(OpenId, Token.AccessTokenJson.RefreshToken, RegionData);

            Debugger.Log(0, "", "FuckBeat\n");
            //This should return 5
            await GetRiotCalls().PerformLcdsHeartBeat(LoginDataPacket.AllSummonerData.Summoner.AcctId, "", HeartbeatCount,
                DateTime.Now.ToString("ddd MMM d yyyy HH:mm:ss 'GMT-0700'"));

            HeartbeatCount++;
        }

        public void OnMessage(Jid jid, string message)
        {
            var findPlayer = Players.FirstOrDefault(x => x.JidAsString == jid.PlayerJid);
            findPlayer?.Messages.Add(new KeyValuePair<string, string>(jid.PlayerJid, message));
        }

        public void OnPresRecieve(Presence pres)
        {
            var findPlayer = Players.FirstOrDefault(x => x.JidAsString == pres.FromJid.PlayerJid);
            if (findPlayer != null)
            {
                var update = ParsePresence(findPlayer, pres.Status);
                update.IsOnline = pres.PresenceType == PresenceType.Available && pres.PresenceShow != PresenceShow.Mobile;
                update.IsAway = pres.PresenceShow != PresenceShow.Chat;
                update.Mobile = false;

                Players.Remove(findPlayer);
                Players.Add(update);
                OnUpdatePlayer?.Invoke(this, update);
            }
            else
            {
                if (!Players.Exists(x => x.JidAsString == pres.FromJid.PlayerJid))
                {
                    var chatPlayerItem = new ChatPlayerItem
                    {
                        JidAsString = pres.FromJid.PlayerJid,
                        IsOnline = pres.PresenceType == PresenceType.Available && pres.PresenceShow != PresenceShow.Mobile,
                        Mobile = false
                    };
                    var update = ParsePresence(chatPlayerItem, pres.Status);
                    update.IsOnline = true;
                    update.IsAway = pres.PresenceShow != PresenceShow.Chat;

                    Players.Add(update);
                }
            }
        }

        public void OnMobilePresRecieve(Presence pres)
        {
            var findPlayer = Players.FirstOrDefault(x => x.JidAsString == pres.FromJid.PlayerJid);
            if (findPlayer != null)
            {
                var update = findPlayer;
                update.IsOnline = pres.PresenceType == PresenceType.Available && pres.PresenceShow != PresenceShow.Mobile;
                update.IsAway = true;
                update.Mobile = true;

                Players.Remove(findPlayer);
                Players.Add(update);
                OnUpdatePlayer?.Invoke(this, update);
            }
            else
            {
                if (Players.Exists(x => x.JidAsString == pres.FromJid.PlayerJid))
                    return;

                var chatPlayerItem = new ChatPlayerItem
                {
                    JidAsString = pres.FromJid.PlayerJid,
                    IsOnline = pres.PresenceType == PresenceType.Available && pres.PresenceShow != PresenceShow.Mobile,
                    Mobile = true,
                    IsAway = pres.PresenceShow != PresenceShow.Chat
                };

                Players.Add(chatPlayerItem);
            }
        }

        public void OnRostRecieve(Jid jid)
        {
            if (!Players.Exists(x => x.JidAsString == jid.PlayerJid))
            {
                var chatPlayerItem = new ChatPlayerItem
                {
                    JidAsString = jid.PlayerJid,
                    IsOnline = false,
                    Group = jid.Group,
                    Username = jid.SumName,
                };
                Players.Add(chatPlayerItem);
            }
            else
            {
                var upt = Players.First(x => x.JidAsString == jid.PlayerJid);
                upt.Group = jid.Group;
                upt.Username = jid.SumName;
            }
        }

        internal string GetPresence()
        {
            var totalWins = LoginDataPacket.PlayerStatSummaries.PlayerStatSummarySet.Sum(set => set.Wins);
            return "<body>" +
                   "<profileIcon>" + LoginDataPacket.AllSummonerData.Summoner.ProfileIconId + "</profileIcon>" +
                   "<level>" + LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel + "</level>" +
                   "<wins>" + totalWins + "</wins>" +
                   
                   /*
                   (PlayerIsRanked ?
                       "<queueType /><rankedLosses>0</rankedLosses><rankedRating>0</rankedRating><tier>UNRANKED</tier>" + //Unused?
                       "<rankedLeagueName>" + Context.LeagueName + "</rankedLeagueName>" +
                       "<rankedLeagueDivision>" + Context.Tier.Split(' ')[1] + "</rankedLeagueDivision>" +
                       "<rankedLeagueTier>" + Context.Tier.Split(' ')[0] + "</rankedLeagueTier>" +
                       "<rankedLeagueQueue>RANKED_SOLO_5x5</rankedLeagueQueue>" +
                       "<rankedWins>" + 999 + "</rankedWins>" : "") + //*/
                   "<gameStatus>outOfGame</gameStatus>" +
                   "<statusMsg>" + "Using IcyWind 2018" + "</statusMsg>" + 
                   "</body>";
        }

        internal static ChatPlayerItem ParsePresence(ChatPlayerItem player, string presence)
        {
            player.RawPresence = presence; //For debugging
            using (XmlReader reader = XmlReader.Create(new StringReader(presence)))
            {
                while (reader.Read())
                {
                    if (!reader.IsStartElement()) continue;

                    #region Parse Presence

                    if (reader.Name == "profileIcon")
                    {
                        reader.Read();
                        player.ProfileIcon = Convert.ToInt32(reader.Value);
                    }
                    else if (reader.Name == "level")
                    {
                        reader.Read();
                        player.Level = Convert.ToInt32(reader.Value);
                    }
                    else if (reader.Name == "wins")
                    {
                        reader.Read();
                        player.Wins = Convert.ToInt32(reader.Value);
                    }
                    else if (reader.Name == "leaves")
                    {
                        reader.Read();
                        player.Leaves = Convert.ToInt32(reader.Value);
                    }
                    else if (reader.Name == "rankedWins")
                    {
                        reader.Read();
                        player.RankedWins = Convert.ToInt32(reader.Value);
                    }
                    else if (reader.Name == "timeStamp")
                    {
                        reader.Read();
                        player.Timestamp = Convert.ToInt64(reader.Value);
                    }
                    else if (reader.Name == "statusMsg")
                    {
                        reader.Read();
                        player.Status = reader.Value;
                    }
                    else if (reader.Name == "gameStatus")
                    {
                        reader.Read();
                        player.GameStatus = reader.Value;
                    }
                    else if (reader.Name == "skinname")
                    {
                        reader.Read();
                        player.Champion = reader.Value;
                    }
                    else if (reader.Name == "rankedLeagueName")
                    {
                        reader.Read();
                        player.LeagueName = reader.Value;
                    }
                    else if (reader.Name == "rankedLeagueTier")
                    {
                        reader.Read();
                        player.LeagueTier = reader.Value;
                    }
                    else if (reader.Name == "rankedLeagueDivision")
                    {
                        reader.Read();
                        player.LeagueDivision = reader.Value;
                    }

                    #endregion Parse Presence
                }
            }
            return player;
        }
    }
}