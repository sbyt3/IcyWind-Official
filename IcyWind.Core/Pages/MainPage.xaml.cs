using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using IcyWind.Chat;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.Chat;
using IcyWind.Core.Pages.IcyWindPages;

namespace IcyWind.Core.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        internal Dictionary<string, ChatBoxControl> ChatControls = new Dictionary<string, ChatBoxControl>();

        public MainPage()
        {
            InitializeComponent();
            var home = new FadeLabel
            {
                Content = "home",
                Margin = new Thickness(5, 0, 0, 0)
            };
            home.MouseDown += HomeButton_OnMouseDown;
            Header.Children.Add(home);

            var play = new FadeLabel
            {
                Content = "play",
                Margin = new Thickness(5, 0, 0, 0)
            };
            play.MouseDown += PlayButton_OnMouseDown;
            Header.Children.Add(play);

            var profile = new FadeLabel
            {
                Content = "profile",
                Margin = new Thickness(5, 0, 0, 0)
            };
            profile.MouseDown += NotReadyFeature;
            Header.Children.Add(profile);

            var shop = new FadeLabel
            {
                Content = "shop",
                Margin = new Thickness(5, 0, 0, 0)
            };
            shop.MouseDown += NotReadyFeature;
            Header.Children.Add(shop);

            var settings = new FadeLabel
            {
                Content = "settings",
                Margin = new Thickness(5, 0, 0, 0)
            };
            settings.MouseDown += NotReadyFeature;
            Header.Children.Add(settings);

            if (StaticVars.AccountInfo.IsDev)
            {
                var devLabel = new FadeLabel
                {
                    Content = "dev",
                    Margin = new Thickness(5, 0, 0, 0)
                };
                devLabel.MouseDown += DevLabel_MouseDown;
                Header.Children.Add(devLabel);
            }

            UserInterfaceCore.MainPage = this;
            Load();
            foreach (var player in StaticVars.ActiveClient.Players)
            {
                var chatItem = new SmallChatItem
                {
                    PlayerNameLabel = {Content = player.Username},
                    StatusLabel = {Content = player.Status},
                    Tag = player
                };
                chatItem.MouseDoubleClick += item_MouseDoubleClick;

                if (player.Mobile)
                {
                    chatItem.StatusEllipse.Fill = Brushes.Orange;
                }
                else if (player.IsOnline && player.IsAway)
                {
                    chatItem.StatusEllipse.Fill = Brushes.Red;
                }
                else if (player.IsOnline)
                {
                    chatItem.StatusEllipse.Fill = Brushes.Green;
                }
                else
                {
                    chatItem.StatusEllipse.Fill = Brushes.Gray;
                }
                ChatStackPanel.Children.Add(chatItem);
            }
            StaticVars.ActiveClient.OnUpdatePlayer += OnUpdatePlayer;
        }

        private void DevLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserInterfaceCore.ChangeMainPageView<DevPage>();
            HideHeader();
        }

        public void Load()
        {
            ProfileImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "Icons", $"{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId}.png")));
            LevelLabel.Content = StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints
                .SummonerLevel;
            NameLabel.Content = StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.Name;
            ExpLabel.Content =
                StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.ExpPoints +
                "/" +
                StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.ExpToNextLevel;
            ExpProgressBar.Maximum = StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints
                .ExpToNextLevel;
            ExpProgressBar.Value =
                StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.ExpPoints;
            BlueEssLabel.Content = "?";
            RitoGemsLabel.Content = "?";
        }

        private void HeaderGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (UserInterfaceCore.SelectedMainPage == typeof(HomePage))
                return;

            HideHeader();
        }

        private void HeaderTriggerGrid_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ShowHeader();
        }

        #region Animations
        private void ChatTriggerGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ChatTriggerGrid.Visibility = Visibility.Hidden;

            var moveAnimation = new ThicknessAnimation(new Thickness(0, ChatGrid.Margin.Top, 0, 30), TimeSpan.FromSeconds(0.25));
            ChatGrid.BeginAnimation(MarginProperty, moveAnimation);
        }

        private void ChatGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ChatTriggerGrid.Visibility = Visibility.Visible;

            var moveAnimation = new ThicknessAnimation(new Thickness(0, ChatGrid.Margin.Top, -190, 30), TimeSpan.FromSeconds(0.25));
            ChatGrid.BeginAnimation(MarginProperty, moveAnimation);
        }

        private void ShowHeader()
        {
            var moveAnimation = new ThicknessAnimation(new Thickness(0, 30, 0, 0), TimeSpan.FromSeconds(0.25));
            HeaderGrid.BeginAnimation(MarginProperty, moveAnimation);
            moveAnimation = new ThicknessAnimation(new Thickness(0, 130, ChatGrid.Margin.Right, 30), TimeSpan.FromSeconds(0.25));
            ChatGrid.BeginAnimation(MarginProperty, moveAnimation);

            var fadeOutAnimation = new DoubleAnimation(0, TimeSpan.FromSeconds(0.25));
            TrianglePoly.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void HideHeader()
        {
            var moveAnimation = new ThicknessAnimation(new Thickness(0, -60, 0, 0), TimeSpan.FromSeconds(0.25));
            HeaderGrid.BeginAnimation(MarginProperty, moveAnimation);
            moveAnimation = new ThicknessAnimation(new Thickness(0, 40, ChatGrid.Margin.Right, 30), TimeSpan.FromSeconds(0.25));
            ChatGrid.BeginAnimation(MarginProperty, moveAnimation);

            var fadeInAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(0.25));
            TrianglePoly.BeginAnimation(OpacityProperty, fadeInAnimation);
        }
        #endregion Animations

        private void HomeButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UserInterfaceCore.ChangeMainPageView<HomePage>();
            ShowHeader();
        }

        private void PlayButton_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            UserInterfaceCore.ChangeMainPageView<PlayPage>();
            HideHeader();
        }

        private void NotReadyFeature(object sender, MouseButtonEventArgs e)
        {
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NotFinishedFeature"));
        }

        private void ChangeStatus(object sender, MouseButtonEventArgs e)
        {
            var status = (Ellipse)sender;

            StaticVars.ActiveClient.IsAway = false;
            var moveAnimation = new ThicknessAnimation(new Thickness(22, 0, 0, 2), TimeSpan.FromSeconds(0.25));
            if (status.Name == "OnlineStatusEllipse")
            {

            }
            else if (status.Name == "BusyStatusEllipse")
            {
                moveAnimation = new ThicknessAnimation(new Thickness(88, 0, 0, 2), TimeSpan.FromSeconds(0.25));
                StaticVars.ActiveClient.IsAway = true;
            }
            else
            {
                moveAnimation = new ThicknessAnimation(new Thickness(152.5, 0, 0, 2), TimeSpan.FromSeconds(0.25));
            }
            StatusRectangle.BeginAnimation(MarginProperty, moveAnimation);
        }



        void OnUpdatePlayer(object sender, ChatPlayerItem e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
            {
                SmallChatItem item = null;
                foreach (SmallChatItem items in ChatStackPanel.Children)
                {
                    if ((string)items.PlayerNameLabel.Content == e.Username)
                        item = items;
                }

                if (item == null)
                {
                    item = new SmallChatItem {Tag = e};
                    item.MouseDoubleClick += item_MouseDoubleClick;
                    ChatStackPanel.Children.Add(item);
                }

                item.PlayerNameLabel.Content = e.Username;
                item.StatusLabel.Content = e.Status;
                var converter = new BrushConverter();

                if (!e.IsOnline)
                    ChatStackPanel.Children.Remove(item);
                else if (e.GameStatus == "outOfGame" && !e.IsAway)
                    item.StatusEllipse.Fill = (Brush)converter.ConvertFromString("#2ecc71");
                else
                    item.StatusEllipse.Fill = (Brush)converter.ConvertFromString("#e74c3c");

                foreach (PlayerChatControl items in PlayerChatStackPanel.Children)
                {
                    if ((string)items.PlayerNameLabel.Content == e.Username)
                    {
                        items.StatusEllipse.Stroke = null;
                        if (e.Mobile)
                        {
                            items.StatusEllipse.Fill = Brushes.Orange;
                        }
                        else if (!e.IsOnline)
                        {
                            items.StatusEllipse.Fill = (Brush)converter.ConvertFromString("#02000000");
                            items.StatusEllipse.Stroke = (Brush)converter.ConvertFromString("#FFA0A0A0");
                        }
                        else if (e.GameStatus == "outOfGame" && !e.IsAway)
                        {
                            items.StatusEllipse.Fill = (Brush)converter.ConvertFromString("#2ecc71");
                        }
                        else
                        {
                            items.StatusEllipse.Fill = (Brush)converter.ConvertFromString("#e74c3c");
                        }
                    }
                }
            }));
        }

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (SmallChatItem)sender;
            var player = (ChatPlayerItem)item.Tag;

            foreach (PlayerChatControl items in PlayerChatStackPanel.Children)
            {
                if ((string)items.PlayerNameLabel.Content == player.Username && items.Visibility != Visibility.Collapsed)
                    return;
            }

            var playerControl = new PlayerChatControl
            {
                Tag = player,
                PlayerNameLabel = {Content = item.PlayerNameLabel.Content },
                StatusEllipse = {Fill = item.StatusEllipse.Fill},
                Margin = new Thickness(5, 0, 0, 0)
            };
            playerControl.MouseDown += PlayerControl_MouseDown;
            PlayerChatStackPanel.Children.Add(playerControl);
        }

        void PlayerControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var playerControl = (PlayerChatControl)sender;
            var item = (ChatPlayerItem)playerControl.Tag;
            ChatBoxControl chatControl;
            if (!ChatControls.ContainsKey(item.JidAsString))
            {
                chatControl = new ChatBoxControl(new Jid(item.JidAsString));
                StaticVars.ActiveClient.XmppClient.OnMessageRecieved += chatControl.OnMessage;
                chatControl.HideButton.Click += (o, args) => { HolderGrid.Children.Remove(chatControl); };
                chatControl.RemoveButton.Click += (o, args) =>
                {
                    HolderGrid.Children.Remove(chatControl);
                    ChatControls.Remove(item.JidAsString);
                    StaticVars.ActiveClient.XmppClient.OnMessageRecieved -= chatControl.OnMessage;
                };
                ChatControls.Add(item.JidAsString, chatControl);
                HolderGrid.Children.Add(chatControl);
            }
            else
            {
                chatControl = ChatControls[item.JidAsString];
                var currentName = (string)chatControl.PlayerNameLabel.Content;
                if (currentName == item.Username)
                {
                    if (chatControl.IsVisible)
                    {
                        HolderGrid.Children.Remove(chatControl);
                    }
                    else
                    {
                        HolderGrid.Children.Add(chatControl);
                    }
                    return;
                }
            }

            Panel.SetZIndex(chatControl, 1);

            chatControl.PlayerNameLabel.Content = item.Username;

            chatControl.HorizontalAlignment = HorizontalAlignment.Left;
            chatControl.VerticalAlignment = VerticalAlignment.Bottom;
            Point relativePoint = playerControl.TransformToAncestor(UserInterfaceCore.MainPage).Transform(new Point(0, 0));
            chatControl.Margin = new Thickness(relativePoint.X, 0, 0, 30);
        }

    }
}
