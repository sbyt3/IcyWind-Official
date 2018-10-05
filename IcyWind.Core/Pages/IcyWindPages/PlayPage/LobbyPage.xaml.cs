using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IcyWind.Chat;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.clientfacade.domain;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.serviceproxy;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.Lobby;
using Newtonsoft.Json;
using RtmpSharp.Messaging;
using WebSocketSharp;

namespace IcyWind.Core.Pages.IcyWindPages.PlayPage
{
    /// <summary>
    /// Interaction logic for LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : UserControl
    {
        private bool _autoAccept = true;
        private bool _hasLoaded = false;
        private int _id;
        private Jid _roomJid;

        public LobbyPage()
        {
            InitializeComponent();
            //SumSpellData
        }

        private async void ReadRtmpResp(object sender, MessageReceivedEventArgs eventArgs)
        {
            if (eventArgs.Body is LcdsServiceProxyResponse proxy)
            {
                if (proxy.MessageId == null && proxy.MethodName == "tbdGameDtoV1" && proxy.ServiceName == "teambuilder-draft")
                {
                    var messageData = JsonConvert.DeserializeObject<PartyPhaseMessage>(
                        Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String(proxy.Payload))));

                    if (messageData.PhaseName == "MATCHMAKING")
                    {
                        await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => StartGameButton.Content = "Matchmaking"));
                    }
                    else if (messageData.PhaseName == "AFK_CHECK")
                    {
                        if (_autoAccept)
                        {
                            var acceptData =
                                await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse(
                                    "teambuilder-draft", "indicateAfkReadinessV1", "{\"afkReady\":true}");

                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
                            {
                                if (acceptData.Status != "ACK")
                                {
                                    UserInterfaceCore.HolderPage.ShowNotification(
                                        UserInterfaceCore.ShortNameToString("UnknownResult"));
                                }

                                UserInterfaceCore.Flash?.Invoke();
                                UserInterfaceCore.HolderPage.ShowNotification(
                                    UserInterfaceCore.ShortNameToString("AcceptedGame"));
                            }));
                        }
                        else
                        {
                            //U piece of shit. No. just no
                        }
                    }
                    else if (messageData.PhaseName == "CHAMPION_SELECT")
                    {
                        await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
                        {
                            //Really bad method. Don't think that this can play more than one game at a time. Will fix
                            UserInterfaceCore.ChangeMainPageView<ChampionSelectPage>(messageData);
                        }));
                    }
                }
            }
        }

        public async void Load(int id)
        {
            _id = id;
            if (_hasLoaded)
            {
                var changeData = JsonConvert.SerializeObject(new SetGameType
                {
                    BotDifficulty = null,
                    GameCustomization = null,
                    GameType = string.Empty,
                    MaxPartySize = 5,
                    QueueId = id
                });

                var change = JsonConvert.SerializeObject(new BodyHelper
                {
                    body = changeData,
                    method = "PUT",
                    url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/gamemode"
                });

                var changeResult = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                    change);

                return;
            }

            _hasLoaded = true;
            TeamListView.Items.Clear();
            StaticVars.ActiveClient.RiotConnection.MessageReceived += ReadRtmpResp;
            var gamedata = JsonConvert.SerializeObject(new SetGameType
            {
                BotDifficulty = null,
                GameCustomization = null,
                GameType = string.Empty,
                MaxPartySize = 5,
                QueueId = id
            });

            var sendData = JsonConvert.SerializeObject(new BodyHelper
            {
                body = gamedata,
                method = "PUT",
                url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/gamemode"
            });

            var party = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                sendData);


            var ready = JsonConvert.SerializeObject(new BodyHelper
            {
                body = "\"open\"",
                method = "PUT",
                url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/members/{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Puuid:D}/ready"
            });
            await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                ready);

            var data = JsonConvert.DeserializeObject<PartyPayload>(party.Payload);

            StaticVars.ActiveClient.CurrentParty = data;

            _roomJid = new Jid(data.Payload.CurrentParty.Chat.Jid + "@sec.pvp.net");
            StaticVars.ActiveClient.XmppClient.JoinRoom(_roomJid);
            StaticVars.ActiveClient.XmppClient.OnMessageRecieved += OnMessage;

            var loader = new Thread(async () =>
            {
                Thread.Sleep(1000);


                var sendData2 = JsonConvert.SerializeObject(new BodyHelper
                {
                    body = "\"open\"",
                    method = "PUT",
                    url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/partytype"
                });

                await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                    sendData2);
            });
            loader.Start();


            var lobby = new FadeLabel
            {
                Content = "return to lobby",
                Margin = new Thickness(5, 0, 0, 0)
            };
            UserInterfaceCore.MainPage.RightHeader.Children.Add(lobby);
            lobby.MouseDown += Lobby_MouseDown;
            TeamListView.Items.Add(new TeamControl
            {
                ProfileIcon =
                {
                    Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation,
                        "IcyWindAssets", "Icons",
                        $"{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId}.png")))
                },
                SumId = { Content = StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.SumId },
                SummonerName = {Content = StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Name}
            });
            StaticVars.ActiveClient.XmppClient.SetPresence(NewPres(), PresenceType.Available, PresenceShow.Chat);
        }

        //This creates an open party. I should really fix this
        private string NewPres()
        {
            return "<body>" +
                   "<statusMsg>Reported</statusMsg>" +
                   "<mapId/>" +
                   "<rankedLeagueName/>" +
                   "<skinVariant/>" +
                   $"<pty>{{\"partyId\":\"{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}\",\"queueId\":{_id},\"summoners\":[{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.AcctId}]}}</pty>" +
                   "<skinname/>" +
                   $"<profileIcon>{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId}</profileIcon>" +
                   "<rankedLeagueDivision/>" +
                   "<isObservable>ALL</isObservable>" +
                   "<gameQueueType>NORMAL</gameQueueType>" +
                   "<gameStatus>hosting_NORMAL</gameStatus>" +
                   $"<level>{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel}</level>" +
                   $"<queueId>{_id}</queueId>" +
                   "<gameMode>CLASSIC</gameMode>" +
                   "</body>";

            // "<rankedLeagueQueue>RANKED_SOLO_5x5</rankedLeagueQueue>" +
            // "<rankedWins>3</rankedWins><" +
            // "championId/>" +
            // "<rankedLeagueTier>PROVISIONAL</rankedLeagueTier>" +
            // "<tier>UNRANKED</tier>" +
            // "<rankedLosses>6</rankedLosses>" +
        }

        private async Task<PartyPayload> Lock(string body)
        {
            var sendData = JsonConvert.SerializeObject(new BodyHelper
            {
                body = $"{body}",
                method = "PUT",
                url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/activityLocked"
            });

            var data = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                sendData);

            return JsonConvert.DeserializeObject<PartyPayload>(data.Payload);
        }

        private void Lobby_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserInterfaceCore.MainPage.ContentContainer.Content = this;
        }

        private async void CancelGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Lock("false");
            var sendData = JsonConvert.SerializeObject(new BodyHelper
            {
                body = "\"DECLINED\"",
                method = "PUT",
                url = $"v1/parties/{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}/members/{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Puuid:D}/role"
            });

            await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("parties.service", "proxy",
                sendData);

            UserInterfaceCore.TypeControls.Remove(typeof(LobbyPage));
            UserInterfaceCore.ChangeMainPageView<HomePage>();
        }

        private async void StartGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            var data = await Lock("true");
            Debugger.Break();
        }

        private void AutoAcceptCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            _autoAccept = true;
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NotFinishedFeature"));
        }

        private void AutoAcceptCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            _autoAccept = false;
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NotFinishedFeature"));
        }

        private void InviteButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("PartyOpen"));
        }

        public void AppendText(string text, Brush color)
        {
            Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                var tr = new TextRange(ChatText.Document.ContentEnd, ChatText.Document.ContentEnd) { Text = text };
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }));
        }

        public void OnMessage(Jid jid, string message)
        {
            if (jid.PlayerJid == _roomJid.PlayerJid && jid.Extra != StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Name)
            {
                Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                {
                    AppendText($"{jid.Extra}: ", Brushes.DarkBlue);
                    AppendText(message, Brushes.Black);
                    AppendText(Environment.NewLine, Brushes.Black);
                }));
            }
        }

        private void ChatButton_OnClick(object sender, RoutedEventArgs e)
        {
            StaticVars.ActiveClient.XmppClient.SendGroupChatMessage(_roomJid, ChatTextBox.Text);
            AppendText("You: ", Brushes.DarkOrchid);
            AppendText(ChatTextBox.Text, Brushes.Black);
            AppendText(Environment.NewLine, Brushes.Black);
            ChatTextBox.Text = string.Empty;
        }

        private void SelectChamp_OnClick(object sender, RoutedEventArgs e)
        {
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NotFinishedFeature"));
        }

        private void InstaCall_OnClick(object sender, RoutedEventArgs e)
        {
            StaticVars.ActiveClient.InstaCall = ChatTextBox.Text;
            ChatTextBox.Text = string.Empty;
        }

        private void ChangeMap_OnClick(object sender, RoutedEventArgs e)
        {
            UserInterfaceCore.ChangeMainPageView<MapSelectionPage>();
        }
    }
}
