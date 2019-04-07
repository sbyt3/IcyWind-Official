using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using IcyWind.Chat;
using IcyWind.Chat.Jid;
using IcyWind.Chat.Messages;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.statistics;
using IcyWind.Core.Logic.Riot.Lobby;
using Newtonsoft.Json;
using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;

namespace IcyWind.Core.Pages.IcyWindPages
{
    /// <summary>
    /// Interaction logic for EndOfGamePage.xaml
    /// </summary>
    public partial class EndOfGamePage : UserControl
    {
        private string MatchStatsOnline;

        private ChatRoom chatRoom;

        public EndOfGamePage(EndOfGameStats stats)
        {
            InitializeComponent();
            chatRoom = StaticVars.ActiveClient.XmppClient.MessageManager.JoinRoom(stats.RoomName + "@sec.pvp.net", stats.RoomPassword);
            StaticVars.ActiveClient.XmppClient.OnMessageRecieved += OnMessage;
            Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
            {
                RenderStats(stats); 
            }));
        }

        private void RenderStats(EndOfGameStats statistics)
        {
            TimeSpan t = TimeSpan.FromSeconds(statistics.GameLength);
            TimeLabel.Content = $"{t.Minutes:D2}:{t.Seconds:D2}";
            ModeLabel.Content = statistics.GameMode;
            TypeLabel.Content = statistics.GameType;

            MatchStatsOnline = "http://matchhistory.na.leagueoflegends.com/en/#match-details/" + StaticVars.ActiveClient.RegionData.RegionName + "/" + statistics.ReportGameId + "/" + statistics.UserId;

            GainedIP.Content = "+" + statistics.IpEarned + " IP";
            TotalIP.Content = statistics.IpEarned.ToString(CultureInfo.InvariantCulture).Replace(".0", "") + " IP Total";
            string game = " XP";
            var allParticipants =
                new List<PlayerParticipantStatsSummary>(statistics.TeamPlayerParticipantStats.ToArray());
            allParticipants.AddRange(statistics.OtherTeamPlayerParticipantStats);

            var readFile = Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell", "summoner.json");
            var text = File.ReadAllText(readFile);
            var internalSpellData = JsonConvert.DeserializeObject<SumSpellData>(text);

            var readFile2 = Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "ChampIcons", "champion.json");
            var text2 = File.ReadAllText(readFile2);
            var internalChampData = JsonConvert.DeserializeObject<ChampionData>(text2);

            foreach (PlayerParticipantStatsSummary summary in allParticipants)
            {
                var playerStats = new EndOfGamePlayer(summary.UserId, summary.GameId, summary.SummonerName, statistics.TeamPlayerParticipantStats.Contains(summary));
                //champions champ = champions.GetChampion(summary.SkinName); //Misleading variable name

                var champ = internalChampData.Data.First(x => x.Value.Key == summary.ChampionId.ToString());
                playerStats.ChampImage.Source = new BitmapImage(new Uri(
                    Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                    champ.Value.Image.Full)));
                playerStats.ChampLabel.Content = summary.SkinName;
                playerStats.PlayerLabel.Content = summary.SummonerName;

                var spell1 = internalSpellData.Data.FirstOrDefault(x => x.Value.Key == summary.Spell1Id.ToString());
                var spell2 = internalSpellData.Data.FirstOrDefault(x => x.Value.Key == summary.Spell1Id.ToString());

                if (File.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                    spell1.Value.Image.Full)))
                {
                    playerStats.Spell1Image.Source = new BitmapImage(new Uri(
                        Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                        spell1.Value.Image.Full)));
                }

                if (File.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                    spell2.Value.Image.Full)))
                {
                    playerStats.Spell2Image.Source = new BitmapImage(new Uri(
                        Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "SumSpell",
                            spell2.Value.Image.Full)));
                }

                double championsKilled = 0;
                double assists = 0;
                double deaths = 0;
                foreach (var stat in summary.Statistics.Where(stat => stat.StatTypeName.ToLower() == "win"))
                {
                    if (summary.SummonerName !=
                        StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Name) continue;
                    GameResultLabel.Content = "Victory";

                }

                if (statistics.Ranked)
                {
                    game = " LP";
                    //GainedXP.Content = (victory ? "+" : "-") + statistics.ExperienceEarned + game;
                    //TotalXP.Content = statistics.ExperienceTotal + game;
                }
                else
                {
                    //GainedXP.Content = "+" + statistics.ExperienceEarned + game;
                    //TotalXP.Content = statistics.ExperienceTotal + game;
                }

                foreach (RawStatDTO stat in summary.Statistics)
                {
                    if (stat.StatTypeName.StartsWith("ITEM") && Math.Abs(stat.Value) > 0)
                    {
                        var item = new Image();
                        if (File.Exists(Path.Combine(StaticVars.IcyWindLocation, "Assets", "item", stat.Value + ".png")))
                        {
                            var UriSource = new System.Uri(Path.Combine(StaticVars.IcyWindLocation, "Assets", "item", stat.Value + ".png"), UriKind.Absolute);
                            item.Source = new BitmapImage(UriSource);
                        }
                        playerStats.ItemsListView.Items.Add(item);
                    }
                    switch (stat.StatTypeName)
                    {
                        case "GOLD_EARNED":
                            if (stat.Value > 0)
                            {
                                playerStats.GoldLabel.Content = $"{stat.Value / 1000:N1}k";
                            }
                            break;

                        case "MINIONS_KILLED":
                            playerStats.CsLabel.Content = stat.Value;
                            break;

                        case "LEVEL":
                            playerStats.LevelLabel.Content = stat.Value;
                            break;

                        case "CHAMPIONS_KILLED":
                            championsKilled = stat.Value;
                            break;

                        case "ASSISTS":
                            assists = stat.Value;
                            break;

                        case "NUM_DEATHS":
                            deaths = stat.Value;
                            break;
                    }
                }
                playerStats.ScoreLabel.Content = championsKilled + "/" + deaths + "/" + assists;
                PlayersListView.Items.Add(playerStats);
            }
            PlayersListView.Items.Insert(allParticipants.Count / 2, new Separator());
            /*
            championSkins skin = championSkins.GetSkin(statistics.SkinIndex);
            try
            {
                if (skin == null)
                    return;

                var skinSource =
                    new System.Uri(Path.Combine(Client.ExecutingDirectory, "Assets", "champions", skin.splashPath),
                        UriKind.Absolute);
                SkinImage.Source = new BitmapImage(skinSource);
            }
            catch (Exception)
            {
            }
            */
        }

        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                StaticVars.ActiveClient.XmppClient.SendMessage(_roomJid, ChatTextBox.Text);
                AppendText("You: ", Brushes.DarkOrchid);
                AppendText(ChatTextBox.Text, Brushes.Black);
                AppendText(Environment.NewLine, Brushes.Black);
                ChatTextBox.Text = string.Empty;
            }
        }

        public void AppendText(string text, Brush color)
        {
            Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
            {
                var tr = new TextRange(ChatText.Document.ContentEnd, ChatText.Document.ContentEnd) { Text = text };
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

        private void ChatButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
