using System;
using System.Collections.Generic;
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
using IcyWind.Auth;
using IcyWind.Auth.Data;
using IcyWind.Core.Controls;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.Crypt;
using IcyWind.Core.Logic.Data;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.IcyWind.Accounts;
using IcyWind.Core.Logic.Riot;
using IcyWind.Core.Logic.Riot.Auth;
using IcyWind.Core.Logic.Riot.Chat;
using Newtonsoft.Json;
using Path = System.IO.Path;

namespace IcyWind.Core.Pages
{
    /// <summary>
    /// Interaction logic for AccountSelectorPage.xaml
    /// </summary>
    public partial class AccountSelectorPage : UserControl
    {
        public AccountSelectorPage()
        {
            InitializeComponent();
            AddNewAccount.Content = UserInterfaceCore.ShortNameToString("AddAccount");
            LoginButton.Content = UserInterfaceCore.ShortNameToString("Login");
            //TODO: Add PBE and KR
            UpdateRegionComboBox.ItemsSource = new[] { "Live" };
            UpdateRegionComboBox.SelectedItem = "Live";
            RegionComboBox.SelectedItem = "NA";
            UsernameImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "ic_account_circle_black_36dp.png")));
            PasswordImg.Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "ic_vpn_key_36pt.png")));

            foreach (var account in StaticVars.UserClientList)
            {
                //TODO: Get the chat status
                UserListView.Items.Add(new UserAccount(account.LoginDataPacket.AllSummonerData.Summoner.Name,
                    account.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel.ToString(),
                    account.RegionData.RegionName,
                    "IcyWind 2018",
                    account.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId.ToString(),
                    Brushes.Green, 
                    account));
            }
        }

        public void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            LoginGrid.Visibility = Visibility.Visible;
        }

        public void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("FeatureNotImplemented"));
        }

        private void Reload_OnClick(object sender, RoutedEventArgs e)
        {
            UserListView.Items.Clear();

            foreach (var account in StaticVars.UserClientList)
            {
                //TODO: Get the chat status
                UserListView.Items.Add(new UserAccount(account.LoginDataPacket.AllSummonerData.Summoner.Name,
                    account.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel.ToString(),
                    account.RegionData.RegionName,
                    "IcyWind 2018",
                    account.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId.ToString(),
                    Brushes.Green,
                    account));
            }
        }


        private void UpdateRegionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((string)UpdateRegionComboBox.SelectedValue)
            {
                case "PBE":
                    RegionComboBox.ItemsSource = new[] { "PBE" };
                    RegionComboBox.SelectedItem = "PBE";
                    LoginUsernameBox.Visibility = Visibility.Visible;
                    LoginPasswordBox.Visibility = Visibility.Visible;
                    break;

                case "KR":
                    RegionComboBox.ItemsSource = new[] { "KR" };
                    RegionComboBox.SelectedItem = "KR";
                    LoginUsernameBox.Visibility = Visibility.Visible;
                    LoginPasswordBox.Visibility = Visibility.Visible;
                    break;

                case "Live":
                    RegionComboBox.ItemsSource = new[] { "BR", "EUNE", "EUW", "JP", "LA1", "LA2", "NA", "OC1", "RU", "TR" };
                    RegionComboBox.SelectedItem = "NA";
                    LoginUsernameBox.Visibility = Visibility.Visible;
                    LoginPasswordBox.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void AddNewAccount_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO: put this in another function
                var username = LoginUsernameBox.Text;
                var password = LoginPasswordBox.Password;
                var region = RegionComboBox.SelectedValue.ToString();
                var regionData = RiotClientData.ReadSystemRegionData(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml"), RegionComboBox.SelectedValue.ToString());
                var thread = new Thread((async() =>
                {
                    var openId = RiotAuth.GetOpenIdConfig();
                    await RiotAuth.Login(this, Dispatcher, username, password, region, openId, true, () => Reload_OnClick(this, null));
                }));
                thread.Start();
            }
            catch
            {
                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("LoginErrorGeneric"));
            }

            LoginGrid.Visibility = Visibility.Hidden;
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO: Create a login bypass -bpz
                var username = LoginUsernameBox.Text;
                var password = LoginPasswordBox.Password;
                var regionData = RiotClientData.ReadSystemRegionData(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml"), RegionComboBox.SelectedValue.ToString());
                var region = RegionComboBox.SelectedValue.ToString();
                var thread = new Thread((async () =>
                {
                    var openId = RiotAuth.GetOpenIdConfig();
                    await RiotAuth.Login(this, Dispatcher, username, password, region, openId, false, () => Reload_OnClick(this, null));

                }));
                thread.Start();
            }
            catch
            {
                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("LoginErrorGeneric"));
            }

            LoginGrid.Visibility = Visibility.Hidden;
        }
    }
}
