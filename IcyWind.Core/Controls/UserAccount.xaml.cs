using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Pages;
using IcyWind.Core.Pages.IcyWindPages;

namespace IcyWind.Core.Controls
{
    /// <summary>
    /// Interaction logic for UserAccount.xaml
    /// </summary>
    public partial class UserAccount : UserControl
    {

        public UserClient Account;

        public UserAccount(string user, string level, string region, string status, string icon, Brush statusColor, UserClient account)
        {
            InitializeComponent();
            PlayerName.Content = user;
            RegionLabel.Content = region;
            LevelLabel.Content = level;
            PlayerStatus.Content = status;
            StatusColour.Fill = statusColor;
            Account = account;

            ProfileImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "Icons", $"{icon}.png")));
        }

        private void ProfileImageContainer_OnClick(object sender, RoutedEventArgs e)
        {
            StaticVars.ActiveClient = Account;
            UserInterfaceCore.ChangeView(typeof(MainPage));
            UserInterfaceCore.ChangeMainPageView<HomePage>();
        }
    }
}
