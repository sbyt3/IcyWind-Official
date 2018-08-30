using IcyWind.HostViews;
using System;
using System.AddIn.Hosting;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using IcyWind.Core;
using IcyWind.Core.Update.IcyWind;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using log4net;
using Newtonsoft.Json;
using IMainHostView = IcyWind.HostViews.IMainHostView;

namespace IcyWind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainWindow));

        private IMainHostView mainHostView;

       public MainWindow()
        {
            Loaded += Load;
            InitializeComponent();
            LanguageHelper.SetResource("English");
        }

        public void Load(object sender, RoutedEventArgs routedEventArgs)
        {
            Hide();
            var t = new Thread(async () =>
            {
                // Get add-in pipeline folder (the folder in which this application was launched from)
                var appPath = Path.Combine(Environment.CurrentDirectory, "core");
                using (var client = new WebClient())
                {
                    //This handles the IcyWind.Core verification process. 
                    if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Certificates")))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Certificates"));
                    }
                    else if (File.Exists(
                        Path.Combine(Environment.CurrentDirectory, "Certificates", "IcyWindRootCA.cer")))
                    {
                        File.Delete(Path.Combine(Environment.CurrentDirectory, "Certificates", "IcyWindRootCA.cer"));
                    }
                    //Download the RootCert from the CDN
                    client.DownloadFile("https://cdn.icywindclient.com/IcyWindRootCA.cer", Path.Combine(Environment.CurrentDirectory, "Certificates", "IcyWindRootCA.cer"));

                    //This handles updates and installs. Right now only installs
                    var latest = client.DownloadString("https://cdn.icywindclient.com/latest.txt");
                    var json = JsonConvert.DeserializeObject<IcyWindVersionJson>(latest);

                    if (!Directory.Exists(appPath))
                    {
                        //Load the installer
                        InstallWindow install = null;
                        await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
                        {
                            install = new InstallWindow(this);
                            install.Show();
                        }));
                        if (install != null)
                        {
                            var tr = await install.Load(json, true);
                        }
                    }
                }

                //This verifies the certificate that was downloaded. It should not be installed into the
                //user's certificate store for security reasons (It is a root CA)
                var root = new X509Certificate2(Path.Combine(Environment.CurrentDirectory, "Certificates", "IcyWindRootCA.cer"));
                var chain = new X509Chain();

                var chainPolicy = new X509ChainPolicy
                {
                    RevocationMode = X509RevocationMode.Offline,
                    RevocationFlag = X509RevocationFlag.EntireChain,
                };

                chain.ChainPolicy = chainPolicy;

                var cert = GetAppCertificate(Path.Combine(appPath, "AddIns", "IcyWind.Core", "IcyWind.Core.dll"));
                if (cert != null)
                {
                    chain.ChainPolicy.ExtraStore.Add(cert);
                }
                else
                {
                    ShowUnsignedCore();
                }

                if (!chain.Build(root))
                {

                    //TODO: FIGURE OUT HOW THE FUCK TO DO THIS BECAUSE I'M ALMOST ABOUT TO PUT MY OWN CERT IN
                    var data = JsonConvert.SerializeObject(chain.ChainStatus);
                    ShowUnsignedCore();
                }

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)Show);

                // Rebuild visual add-in pipeline
                var warnings = AddInStore.Rebuild(appPath);
                if (warnings.Length > 0)
                {
                    var msg = warnings.Aggregate(LanguageHelper.ShortNameToString("PipelineRebuildFail"),
                        (current, warning) => current + "\n" + warning);
                    Log.Error("Pipeline rebuild failed. Stopping program");
                    MessageBox.Show(msg);
                    Environment.Exit(5);
                }

                //Load the IcyWind.Core add-in
                var addInTokens =
                    AddInStore.FindAddIn(typeof(IMainHostView), appPath,
                        Path.Combine(appPath, "AddIns", "IcyWind.Core", "IcyWind.Core.dll"),
                        "IcyWind.Core.IcyWind");
                //Prevent other add-ins from being loaded and block start if add-ins are in wrong place
                if (addInTokens.Count > 1)
                {
                    MessageBox.Show(LanguageHelper.ShortNameToString("MoreOneCore"),
                        "IcyWind Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Fatal("More than one IcyWind Core installed.");
                    Environment.Exit(1);
                }
                else
                {
                    var dirs = Directory.GetFiles(Path.Combine(appPath, "AddIns"));
                    if (dirs.Length > 1)
                    {
                        MessageBox.Show(LanguageHelper.ShortNameToString("PluginWrongLocation"));
                        Log.Warn("Plugin installed in wrong location.");
                        Environment.Exit(1);
                    }
                }

                //Get the add-in
                var mainPage = addInTokens.First();

                //Give the add-in full trust to the system
                mainHostView = mainPage.Activate<IMainHostView>(AddInSecurityLevel.FullTrust);

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action) (() =>
                {
                    var hwd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
                    var addInUi = mainHostView.Run("English", Assembly.GetEntryAssembly().Location, hwd);
                    //AddInController controller = AddInController.GetAddInController(addInUi);
                    MainContent.Content = addInUi;
                }));
            });
            t.Start();
        }

        private void ShowUnsignedCore()
        {
#if DEBUG
            var result =
                MessageBox.Show(
                "Untrusted version of IcyWind.Core detected (unsigned). \n" +
                "This may mean that your IcyWind install is infected, corrupted or non-genuine. \n \n" +
                "If you are debugging IcyWind you can safely ignore this error \n" +
                "It is recommended that you exit IcyWind and re-download it from the official website: \n" +
                "https://icywindclient.com \n \n" +
                "To continue running IcyWind (not recommended), press 'Yes' to run IcyWind \n",
                "Unsigned IcyWind.Core",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                MessageBox.Show("IcyWind will now exit");
                Environment.Exit(0);
            }
            Dispatcher.Invoke(Show);
#else
            if (Environment.GetCommandLineArgs().Contains("-overrideUnsigned"))
            {
                var result =
                    MessageBox.Show(
                        "Untrusted version of IcyWind.Core detected (unsigned). \n" +
                        "This may mean that your IcyWind install is infected, corrupted or non-genuine. \n \n" +
                        "If you are debugging IcyWind you can safely ignore this error \n" +
                        "It is recommended that you exit IcyWind and re-download it from the official website: \n" +
                        "https://icywindclient.com \n \n" +
                        "To continue running IcyWind (not recommended), press 'Yes' to run IcyWind \n",
                        "Unsigned IcyWind.Core",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes)
                {
                    MessageBox.Show("IcyWind will now exit");
                    Environment.Exit(0);
                }
            }
            else
            {
                var result =
                    MessageBox.Show(
                        "Untrusted version of IcyWind.Core detected (unsigned). \n" +
                        "This may mean that your IcyWind install is infected, corrupted or non-genuine. \n \n" +
                        "IcyWind will now exit, re-download IcyWind from the official website: \n" +
                        "https://icywindclient.com \n \n" +
                        "To override this error (not-recommended), use the following \n" +
                        "Command Line Argument: \n" +
                        "'-overrideUnsigned'",
                        "Unsigned IcyWind.Core",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                Environment.Exit(0);

            }
#endif
        }

        public X509Certificate2 GetAppCertificate(string filename)
        {
            X509Certificate2 cert = null;
            try
            {
                cert = new X509Certificate2(X509Certificate.CreateFromSignedFile(filename));
            }
            catch (CryptographicException e)
            {
                Debugger.Log(0, "", $"Error {e.GetType()} : {e.Message}");
                Debugger.Log(0, "", "Couldn't parse the certificate. Be sure it is a X.509 certificate");
            }
            return cert;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            TrayIcon.Dispose();
            e.Cancel = mainHostView.Close();
            TrayIcon.Visibility = Visibility.Hidden;
        }

    }
}
