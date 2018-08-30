using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginPage
    {
        private MediaPlayer player;
        public LoginPage()
        {
            //Remove resource dictionaries
            Resources.Clear();
            Resources.MergedDictionaries.Clear();

            //Set the correct resource dictionary
            Resources.MergedDictionaries.Add(UserInterfaceCore.MainLanguage);

            StaticVars.LeagueLocation = RiotClientData.GetLolRootPath();

            if (string.IsNullOrWhiteSpace(StaticVars.IcyWindLocation))
            {
                MessageBox.Show("You shouldn't see this. If you do, blame eddy. The data path didn't get passed. IcyWind will now close", "Path Error", MessageBoxButton.OK);
                StaticVars.Logger.Error("Path not passed to IcyWind.Core");
                Environment.Exit(1);
            }

            InitializeComponent();
            StatusBox.Items.Add(new IcyWindStatus(Brushes.Orange, "IcyWind is in development. Use at your own risk."));
            StatusBox.Items.Add(new IcyWindStatus(Brushes.Green, "Go thank WildBook (TrapBook) for helping out the project"));
            StatusBox.Items.Add(new IcyWindStatus(Brushes.Green, "IcyWind.Auth server running"));
            StatusBox.Items.Add(new IcyWindStatus(Brushes.Green, "Riot servers running"));
            Img.Source = new BitmapImage(new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "morgana_vs_ahri_3.jpg")));
            Video.Source = new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "LND.mp4"));
            Video.LoadedBehavior = MediaState.Manual;
            Video.SpeedRatio = 0.9;
            Video.IsMuted = true;
            Video.Play();
            player = new MediaPlayer();
            player.Open(new Uri(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "LND.wav")));
            player.Volume = 0.2;
            player.Play();
            player.MediaEnded += (sender, args) =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    player.Stop();
                    player.Play();
                    Video.Stop();
                    Video.Play();
                }));
            };
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressBar.Visibility = Visibility.Visible;
            var loginCred = new LoginCredentials
            {
                Username = Username.Text,
                Password = Password.Password
            };
            //Missing information. This is done to save bandwidth for the user.
            //
            if (string.IsNullOrWhiteSpace(loginCred.Username) || string.IsNullOrWhiteSpace(loginCred.PasswordHash))
            {
                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("NoCred"));
                LoginProgressBar.Visibility = Visibility.Hidden;
                return;
            }

            var t = new Thread(async () =>
            {
                var accountInfo = AccountManager.GetAccountInfo(loginCred);
                // Failed login information
                if (!string.IsNullOrWhiteSpace(accountInfo.Error))
                {
                    switch (accountInfo.Error)
                    {
                        case "InvalidCred":
                        case "UserBanned":
                        case "EmailNotVerified":
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                            {
                                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString(accountInfo.Error));
                                LoginProgressBar.Visibility = Visibility.Hidden;
                                Password.Clear();
                            }));
                            break;
                        default:
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                            {
                                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("LoginErrorGeneric"));
                                LoginProgressBar.Visibility = Visibility.Hidden;
                                Password.Clear();
                            }));
                            break;
                    }
                    return;
                }

                if (!string.IsNullOrWhiteSpace(accountInfo.TwoFactor))
                {
                    LoginProgressBar.Visibility = Visibility.Hidden;
                    switch (accountInfo.Error)
                    {
                        //TODO: Implement a way to input a code for 2FA
                        //TODO: Add an option to exclude 2FA for certain known devices of IP ADDRESSES
                        case "CheckEmail":
                        case "CheckText":
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                            {
                                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString(accountInfo.Error));
                                LoginProgressBar.Visibility = Visibility.Hidden;
                            }));
                            break;
                        default:
                            await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                            {
                                UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("LoginErrorGeneric"));
                                LoginProgressBar.Visibility = Visibility.Hidden;
                            }));
                            break;
                    }
                    return;
                }

                //This is the OpenId config used to login. I use a custom
                //Implementation which may not fully represent OpenId but
                //It gets the job done
                var openId = RiotAuth.GetOpenIdConfig();
                //The SHA1 Hash of the password is used as the password
                //To get the accounts from the servers. The MD5 hash of that
                //Password is the AES key. This may need to change to a different
                //Hashing algorithm, and maybe the encryption should include
                //A part form the IcyWindAuth server which is used to decrypt
                var aesKey = MD5.Hash(Password.Password);
                StaticVars.AccountInfo = accountInfo;
                StaticVars.LoginCred = loginCred;
                StaticVars.Password = aesKey;

                //This is done to stop people who are not devs or donators from accessing IcyWind in Beta
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (!accountInfo.IsDev && !accountInfo.IsPaid && false)
                {
                    await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                    {
                        UserInterfaceCore.HolderPage.ShowNotification(UserInterfaceCore.ShortNameToString("FeaturedDonator"));
                    }));
                    return;
                }

                if (!string.IsNullOrWhiteSpace(accountInfo.Accounts))
                {
                    foreach (var account in JsonConvert.DeserializeObject<string[]>(accountInfo.Accounts))
                    {
                        var copyAccount = account;
                        var mm = copyAccount.Length % 4;
                        if (mm > 0)
                        {
                            copyAccount += new string('=', 4 - mm);
                        }
                        var data = Convert.FromBase64String(copyAccount);
                        var accountAsJson = Encoding.UTF8.GetString(data);

                        var decodedString = JsonConvert.DeserializeObject<IcyWindRiotAccountCrypted>(accountAsJson);

                        if (StaticVars.LoginCred.AccountData == null)
                        {
                            StaticVars.LoginCred.AccountData = new List<IcyWindRiotAccountCrypted>();
                        }

                        StaticVars.LoginCred.AccountData.Add(decodedString);
                        var accountDecrypted = AES.DecryptBase64(aesKey, decodedString.CryptString);

                        var accountDetail = JsonConvert.DeserializeObject<IcyWindRiotAccountInfo>(accountDecrypted);
                        await RiotAuth.Login(this, Dispatcher, accountDetail.Username, accountDetail.Password, accountDetail.Region, openId, true, null);
                    }
                }
                else
                {
                    StaticVars.LoginCred.AccountData = new List<IcyWindRiotAccountCrypted>();
                }

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    UserInterfaceCore.ChangeView(typeof(AccountSelectorPage));
                    //Fade out the audio
                    var timer = new System.Timers.Timer {Interval = 50};
                    timer.Elapsed += (obj, evt) =>
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                        {
                            player.Volume -= 0.01;
                            if (!(player.Volume <= 0))
                                return;
                            player.Stop();
                            timer.Stop();
                            Video.Stop();
                        }));
                    };
                    timer.Start();

                    LoginProgressBar.Visibility = Visibility.Hidden;
                }));
            });
            t.Start();
        }


        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Video.Stop();
            Video.Visibility = Visibility.Hidden;
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            var timer = new System.Timers.Timer { Interval = 40 };
            timer.Elapsed += (obj, evt) =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    player.Volume -= 0.01;
                    if (!(player.Volume <= 0))
                        return;
                    Video.Position = TimeSpan.Zero;
                    Video.Visibility = Visibility.Visible;

                    player.Volume = 0.2;

                    timer.Stop();
                    player.Position = TimeSpan.Zero;

                    player.Play();
                    Video.Play();
                }));
            };
            timer.Start();
        }
    }
}
