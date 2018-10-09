using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
using IcyWind.Chat.Jid;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.serviceproxy;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.Lobby;
using IcyWind.Core.Logic.Riot.RiotData;
using Newtonsoft.Json;
using Page = IcyWind.Core.Logic.Riot.RiotData.Page;
using Path = System.IO.Path;

namespace IcyWind.Core.Pages.IcyWindPages
{
    /// <summary>
    /// Interaction logic for ChampionSelectPage.xaml
    /// </summary>
    public partial class ChampionSelectPage : UserControl
    {
        private bool _hasInit = false;
        private readonly SumSpellData _internalSpellData;
        private readonly ChampionData _internalChampData;
        private UserJid _roomJid;
        private List<PlayerItem> _teamPlayerItems;
        private int _selectedWardSkin;
        private long _champId;
        private bool _hasLocked;
        private readonly Champions _champs;
        private int _actId;
        private bool isBlue;
        private Dictionary<string, Page> runePage;
        private int _timeLeft;
        private int _queueId;

        public ChampionSelectPage(PartyPhaseMessage message)
        {
            InitializeComponent();
            _teamPlayerItems = new List<PlayerItem>();
            runePage = new Dictionary<string, Page>();
            _queueId = (int) message.QueueId;

            if (StaticVars.ActiveClient.RunesReforaged.PageSettings.Data.PerShardPerkBooks.Region.First().Value
                    .ToObject<Region>().Pages.Length == 0)
            {
                var runeJson = File.ReadAllText(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "Runes", "def.json"));
                foreach (var page in JsonConvert.DeserializeObject<Page[]>(runeJson))
                {
                    Perks.Items.Add(page.Name);
                    runePage.Add(page.Name, page);
                }
            }
            else
            {
                foreach (var page in StaticVars.ActiveClient.RunesReforaged.PageSettings.Data.PerShardPerkBooks.Region
                    .First().Value
                    .ToObject<Region>().Pages)
                {
                    Perks.Items.Add(page.Name);
                    runePage.Add(page.Name, page);
                }
            }

            Perks.SelectedItem = Perks.Items[0];
            Perks.SelectionChanged += Perks_SelectionChanged;

            _champs = new Champions();



            var readFile = Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell", "summoner.json");
            var text = File.ReadAllText(readFile);
            _internalSpellData = JsonConvert.DeserializeObject<SumSpellData>(text);

            var readFile2 = Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "ChampIcons", "champion.json");
            var text2 = File.ReadAllText(readFile2);
            _internalChampData = JsonConvert.DeserializeObject<ChampionData>(text2);

            foreach (var champ in StaticVars.ActiveClient.ChampionList) //.Where(x => x.Owned || x.FreeToPlay)
            {
                var champConv = _internalChampData.Data.FirstOrDefault(x =>
                    x.Value.Key == champ.ChampionId.ToString());
                var img = new ChampIcon
                {
                    ChampImg = {Source = new BitmapImage(new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets",
                        "ChampIcons", champConv.Value.Image.Full)))},
                    Tag = champ.ChampionId,
                };
                img.MouseDown += Img_MouseDown;
                _champs.ChampView.Items.Add(img);
            }

            ChampionControl.Content = _champs;
            _hasLocked = false;
            Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() => { UpdateChampSelect(message); }));
            StaticVars.ActiveClient.RiotConnection.MessageReceived += RiotConnection_MessageReceived;
        }

        private async void Perks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var page = runePage[Perks.SelectedItem.ToString()];

            var perkHelper = new PerkHelper
            {
                PerkIds = page.SelectedPerkIds,
                PerkStyle = page.PrimaryStyleId,
                PerkSubStyle = page.SubStyleId,
            };

            var data = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("gamecustomization",
                "setgamecustomization",
                $"{{\"category\":\"perks\",\"content\":\"{JsonConvert.SerializeObject(perkHelper)}\",\"queueType\":{_queueId}}}");
            Debugger.Log(0, "", data.Payload);
            //{"category":"perks","content":"","queueType":430}
        }

        private async void RiotConnection_MessageReceived(object sender, RtmpSharp.Messaging.MessageReceivedEventArgs e)
        {
            if (e.Body is LcdsServiceProxyResponse proxy)
            {
                if (proxy.MessageId == null && proxy.MethodName == "tbdGameDtoV1" && proxy.ServiceName == "teambuilder-draft")
                {
                    var messageData = JsonConvert.DeserializeObject<PartyPhaseMessage>(
                        Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String(proxy.Payload))));
                    if (messageData.PhaseName == "CHAMPION_SELECT")
                    {
                        await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => { UpdateChampSelect(messageData); }));
                    }
                }
            }
        }

        private async void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ChampIcon img)
            {
                _champId = (long)img.Tag;
                Debugger.Log(0, "", "Picked: " + _champId);

                if (_hasLocked)
                    return;

                var data = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("teambuilder-draft",
                    "updateActionV1", $"{{\"actionId\":{_actId}, \"championId\":{_champId}, \"completed\":false}}");
            }
        }

        public async void UpdateChampSelect(PartyPhaseMessage message)
        {
            if (message.PhaseName == "CHAMPION_SELECT")
            {
                #region ChampSelectInit

                if (!_hasInit)
                {
                    _hasInit = true;

                    //TODO: try to change this JWT to unlock all champs
                    await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCall("teambuilder-draft",
                        "updateInventoryV1", $"{{\"simplifiedInventoryJwt\":\"{StaticVars.ActiveClient.InvToken}\"}}");

                    _roomJid = new UserJid(message.ChampionSelectState.TeamChatRoomId + "@champ-select.pvp.net");
                    StaticVars.ActiveClient.XmppClient.JoinRoom(_roomJid);
                    StaticVars.ActiveClient.XmppClient.OnMessageRecieved += OnMessage;
                    var t = new Timer
                    {
                        Interval = 1000
                    };
                    _timeLeft = (int)(message.ChampionSelectState.CurrentTimeRemainingMillis / 1000);
                    t.Elapsed += (sender, args) =>
                    {
                        _timeLeft--;
                        Dispatcher.BeginInvoke(DispatcherPriority.Render,
                            (Action) (() => { CountdownLabel.Content = _timeLeft; }));
                    };
                    t.Start();
                    //TODO: Init perks

                    if (!string.IsNullOrWhiteSpace(StaticVars.ActiveClient.InstaCall))
                    {
                        StaticVars.ActiveClient.XmppClient.SendGroupChatMessage(_roomJid, StaticVars.ActiveClient.InstaCall);
                        StaticVars.ActiveClient.InstaCall = string.Empty;
                    }

                    foreach (var addAlliedTeam in message.ChampionSelectState.Cells.AlliedTeam)
                    {
                        var sumName = await StaticVars.ActiveClient.GetRiotCalls()
                            .GetSummonerNames(new[] { (double)addAlliedTeam.SummonerId });
                        var sumData = await StaticVars.ActiveClient.GetRiotCalls()
                            .GetAllPublicSummonerDataByName(addAlliedTeam.SummonerName);

                        var spell1 = _internalSpellData.Data.FirstOrDefault(x => x.Value.Key == addAlliedTeam.Spell1Id.ToString());
                        var spell2 = _internalSpellData.Data.FirstOrDefault(x => x.Value.Key == addAlliedTeam.Spell2Id.ToString());

                        if (addAlliedTeam.SummonerName ==
                            StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Name)
                        {
                            _actId = (int)addAlliedTeam.CellId;
                        }

                        var player = new PlayerItem
                        {
                            PlayerNameLabel = { Content = addAlliedTeam.SummonerName },
                            PlayerLeagueLabel =
                            {
                                Content = sumData.Summoner.PreviousSeasonHighestTier + " " +
                                          sumData.SummonerLevelAndPoints.SummonerLevel
                            },
                            SummonerSpell1Image =
                            {
                                Source = new BitmapImage(new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell", spell1.Value.Image.Full))),
                                Tag = addAlliedTeam.Spell1Id
                            },
                            SummonerSpell2Image =
                            {
                                Source = new BitmapImage(new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell", spell2.Value.Image.Full))),
                                Tag = addAlliedTeam.Spell2Id
                            }
                        };
                        if (addAlliedTeam.TeamId == 2)
                        {
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() => RedPanel.Children.Add(player)));
                            isBlue = false;
                        }
                        else
                        {
                            player.RedGrid.Visibility = Visibility.Hidden;
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                                BluePanel.Children.Add(player)));
                            isBlue = true;
                        }
                        _teamPlayerItems.Add(player);
                    }

                    foreach (var enemyTeam in message.ChampionSelectState.Cells.EnemyTeam)
                    {
                        var enemyPlayer = new PlayerItem
                        {
                            PlayerNameLabel = { Content = "Bad people" },
                            PlayerLeagueLabel =
                            {
                                Content = "Unknown"
                            },
                        };
                        if (enemyTeam.TeamId == 2)
                        {
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                                RedPanel.Children.Add(enemyPlayer)));
                        }
                        else
                        {
                            enemyPlayer.RedGrid.Visibility = Visibility.Hidden;

                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                                BluePanel.Children.Add(enemyPlayer)));
                        }
                    }
                    var wardLoader = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse(
                        "wardSkins",
                        "requestSelectedWardSkin", "");

                    Debugger.Log(0, "", wardLoader.Payload + "\n");

                }
                #endregion ChampSelectInit
                else
                {
                    foreach (var addAlliedTeam in _teamPlayerItems)
                    {
                        var spell1 = _internalSpellData.Data.FirstOrDefault(x => x.Value.Key == addAlliedTeam.SummonerSpell1Image.Tag.ToString());
                        var spell2 = _internalSpellData.Data.FirstOrDefault(x => x.Value.Key == addAlliedTeam.SummonerSpell2Image.Tag.ToString());

                        addAlliedTeam.SummonerSpell1Image.Source = new BitmapImage(new Uri(
                            Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                                spell1.Value.Image.Full)));
                        addAlliedTeam.SummonerSpell2Image.Source = new BitmapImage(new Uri(
                            Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                                spell2.Value.Image.Full)));

                        var allyTeam = message.ChampionSelectState.Cells.AlliedTeam.First(x =>
                            x.SummonerName == addAlliedTeam.PlayerNameLabel.Content.ToString());

                        if (allyTeam.ChampionId != 0)
                        {
                            var champ = _internalChampData.Data.FirstOrDefault(x =>
                                x.Value.Key == allyTeam.ChampionId.ToString());

                            addAlliedTeam.ChampionImage.Source = new BitmapImage(new Uri(
                                Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "ChampIcons",
                                    champ.Value.Image.Full)));
                        }

                        if (isBlue)
                        {
                            BluePanel.Children[(int)allyTeam.CellId] = addAlliedTeam;
                        }
                        else
                        {
                            RedPanel.Children[(int) allyTeam.CellId - 5] = addAlliedTeam;
                        }
                    }
                }
            }

            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
            {
                UserInterfaceCore.MainPage.ContentContainer.Content = this;
            }));
        }

        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StaticVars.ActiveClient.XmppClient.SendGroupChatMessage(_roomJid, SendText.Text);
                AppendText("You: ", Brushes.DarkOrchid);
                AppendText(SendText.Text, Brushes.Black);
                AppendText(Environment.NewLine, Brushes.Black);
                SendText.Text = string.Empty;
            }
        }

        public void AppendText(string text, Brush color)
        {
            Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                var tr = new TextRange(ChatBox.Document.ContentEnd, ChatBox.Document.ContentEnd) { Text = text };
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            }));
        }

        public void OnMessage(UserJid jid, string message)
        {
            if (jid.PlayerJid == _roomJid.PlayerJid)
            {
                Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                {
                    AppendText($"{jid.Extra}: ", Brushes.DarkBlue);
                    AppendText(message, Brushes.Black);
                    AppendText(Environment.NewLine, Brushes.Black);
                }));
            }
        }

        private async void LockButton_OnClick(object sender, RoutedEventArgs e)
        {
            _hasLocked = true;

            if (!StaticVars.AccountInfo.IsDev)
            {
                LockButton.IsEnabled = false;
            }

            var data = await StaticVars.ActiveClient.RiotProxyCalls.DoLcdsProxyCallWithResponse("teambuilder-draft",
                "updateActionV1", $"{{\"actionId\":{_actId},\"championId\":{_champId},\"completed\":true}}");
        }
    }
}
