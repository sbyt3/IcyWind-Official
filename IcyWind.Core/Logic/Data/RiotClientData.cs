using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using YamlDotNet.RepresentationModel;

namespace IcyWind.Core.Logic.Data
{
    /// <summary>
    ///     Gathers required data to
    /// </summary>
    public static class RiotClientData
    {
        /// <summary>
        ///     Parser for the System.Yaml file.
        ///     There probably is a better way to do this, but honestly I don't know how so somebody pls help meh
        /// </summary>
        /// <param name="systemFile">The input System.Yaml</param>
        /// <param name="region">The region that the user wants to get data for</param>
        /// <returns>The region data contained in System.Yaml</returns>
        public static RegionData ReadSystemRegionData(string systemFile, string region)
        {
            //Read the System.Yaml file
            var input = new StringReader(File.ReadAllText(systemFile));
            var yaml = new YamlStream();
            yaml.Load(input);

            //Use the rootnode to get the data required
            var mapping = (YamlMappingNode) yaml.Documents[0].RootNode;

            //Open the region information and load the selected region from string
            var regionDataYaml = mapping["region_data"][region];
            var serverRegionDataYaml = regionDataYaml["servers"];

            //Create output and populate it with data
            var regionData = new RegionData
            {
                AvailableLocales = new List<Locales>(),
                DefaultLocale = (Locales) Enum.Parse(typeof(Locales), regionDataYaml["default_locale"].ToString()),
                Rso = new RSO
                {
                    AllowLoginQueueFallback = bool.Parse(regionDataYaml["rso"]["allow_lq_fallback"].ToString()),
                    Kount = new Kount
                    {
                        Collecter = regionDataYaml["rso"]["kount"]["collector"].ToString(),
                        Merchant = int.Parse(regionDataYaml["rso"]["kount"]["merchant"].ToString().Replace("'", ""))
                    },
                    Token = regionDataYaml["rso"]["token"].ToString()
                },
                PlatformId =
                    (RsoPlatformId) Enum.Parse(typeof(RsoPlatformId), regionDataYaml["rso_platform_id"].ToString()),
                Servers = new Servers
                {
                    AccountRecovery = new AccountRecovery
                    {
                        ForgotPasswordUrl = serverRegionDataYaml["account_recovery"]["forgot_password_url"].ToString(),
                        ForgotUsernameUrl = serverRegionDataYaml["account_recovery"]["forgot_username_url"].ToString()
                    },
                    Chat = new Chat
                    {
                        AllowSelfSignedCert =
                            bool.Parse(serverRegionDataYaml["chat"]["allow_self_signed_cert"].ToString()),
                        ChatHost = serverRegionDataYaml["chat"]["chat_host"].ToString(),
                        ChatPort = int.Parse(serverRegionDataYaml["chat"]["chat_port"].ToString())
                    },
                    DiscoverousServiceLocation = serverRegionDataYaml["discoverous_service_location"].ToString(),
                    EmailVerification = new EmailVerification
                    {
                        ExternalUrl = serverRegionDataYaml["email_verification"]["external_url"].ToString()
                    },
                    Entitlements = new Entitlements
                    {
                        ExternalUrl = serverRegionDataYaml["entitlements"]["entitlements_url"].ToString()
                    },
                    Lcds = new Lcds
                    {
                        LcdsHost = serverRegionDataYaml["lcds"]["lcds_host"].ToString(),
                        LcdsPort = int.Parse(serverRegionDataYaml["lcds"]["lcds_port"].ToString()),
                        LoginQueueUrl = serverRegionDataYaml["lcds"]["login_queue_url"].ToString(),
                        UseTls = bool.Parse(serverRegionDataYaml["lcds"]["use_tls"].ToString())
                    },
                    LicenseAgrerementUrls = new LicenseAgrerementUrls
                    {
                        Eula = serverRegionDataYaml["license_agreement_urls"].AllNodes.Contains("eula")
                            ? serverRegionDataYaml["license_agreement_urls"]["eula"].ToString()
                            : "http://leagueoflegends.com/{language}/legal/eula",
                        TermsOfUse = serverRegionDataYaml["license_agreement_urls"]["terms_of_use"].ToString()
                    },
                    Payments = new Payments
                    {
                        PaymentsHost = serverRegionDataYaml["payments"]["payments_host"].ToString()
                    },
                    PreloginConfig = new PreloginConfig
                    {
                        PreloginConfigUrl = serverRegionDataYaml["prelogin_config"]["prelogin_config_url"].ToString()
                    },
                    Rms = new Rms
                    {
                        RmsHeartbeatIntervalSeconds =
                            int.Parse(serverRegionDataYaml["rms"]["rms_heartbeat_interval_seconds"].ToString()),
                        RmsUrl = serverRegionDataYaml["rms"]["rms_url"].ToString()
                    },
                    ServiceStatus = new ServiceStatus
                    {
                        ApiUrl = serverRegionDataYaml["service_status"]["api_url"].ToString(),
                        HumanReadableStatusUrl =
                            serverRegionDataYaml["service_status"]["human_readable_status_url"].ToString()
                    },
                    Store = new Store
                    {
                        StoreUrl = serverRegionDataYaml["store"]["store_url"].ToString()
                    },
                    Voice = new Voice
                    {
                        AccessTokenUrl = serverRegionDataYaml["voice"]["access_token_uri"].ToString(),
                        AuthTokenUrl = serverRegionDataYaml["voice"]["auth_token_uri"].ToString(),
                        UseExternalAuth = bool.Parse(serverRegionDataYaml["voice"]["use_external_auth"].ToString()),
                        VoiceDomain = serverRegionDataYaml["voice"]["voice_domain"].ToString(),
                        VoiceUrl = serverRegionDataYaml["voice"]["voice_url"].ToString()
                    }
                },
                WebRegion = regionDataYaml["web_region"].ToString()
            };

            regionData.AvailableLocales.Clear();
            foreach (var locale in regionDataYaml["available_locales"].AllNodes)
            {
                var localAsEnum = (Locales) Enum.Parse(typeof(Locales),
                    locale.ToString().Replace("[", "").Replace("]", "").Replace(" ", ""));
                if (regionData.AvailableLocales.Contains(localAsEnum))
                    continue;

                regionData.AvailableLocales.Add(localAsEnum);
            }

            return regionData;
        }

        /// <summary>
        /// Some code from Sightstone. Attempts to find the location of League of Legends
        /// </summary>
        /// <returns>The location of League of Legends</returns>
        public static string GetLolRootPath()
        {
            //Check for if IcyWind has been run before. If it has been, this should hold the location of the league of legends install
            if (Registry.CurrentUser.GetSubKeyNames().Contains("Software\\IcyWind"))
            {
                return Registry.CurrentUser.OpenSubKey("Software\\IcyWind")?.GetValue("Path").ToString();
            }

            //F**k we gotta look for league of legends
            //Every goddam path
            //A lot of these locations are from Sightstone, which is dead
            var possiblePaths = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\SightstoneLol", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\RIOT GAMES", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\Wow6432Node\RIOT GAMES", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\RIOT GAMES", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Wow6432Node\Riot Games", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Riot Games\League Of Legends", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games\League Of Legends",  "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games \League Of Legends", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\RIOT GAMES, INC", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\Wow6432Node\RIOT GAMES, INC", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\RIOT GAMES, INC", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Wow6432Node\Riot Games, Inc", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Riot Games, Inc\League Of Legends", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc", "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc\League Of Legends",  "Path"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc \League Of Legends", "Path"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\RIOT GAMES", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\Wow6432Node\RIOT GAMES", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\RIOT GAMES", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Wow6432Node\Riot Games", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Riot Games\League Of Legends", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games\League Of Legends",  "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\RIOT GAMES, INC", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Classes\VirtualStore\MACHINE\SOFTWARE\Wow6432Node\RIOT GAMES, INC", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\RIOT GAMES, INC", "Location"),
                    new Tuple<string, string>(@"HKEY_CURRENT_USER\Software\Wow6432Node\Riot Games, Inc", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Riot Games, Inc\League Of Legends", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc", "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc\League Of Legends",  "Location"),
                    new Tuple<string, string>(@"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Riot Games, Inc \League Of Legends", "Location"),
                };
            foreach (var tuple in possiblePaths)
            {
                try
                {
                    var value = Registry.GetValue(tuple.Item1, tuple.Item2, string.Empty);
                    if (value == null || value.ToString() == string.Empty)
                        continue;

                    var icyWindRegKey = Registry.CurrentUser.CreateSubKey("Software\\IcyWind");
                    icyWindRegKey?.SetValue("Path", value.ToString());

                    return value.ToString();
                }
                catch 
                {
                    //Client.Log(e);
                }
            }

            //Shoot didn't find it

            var findLeagueDialog =
                new OpenFileDialog
                {
                    DefaultExt = ".exe",
                    Filter = "League of Legends|LeagueClient.exe"
                };
            
            var result = findLeagueDialog.ShowDialog();
            if (result != true)
                return string.Empty;

            var key = Registry.CurrentUser.CreateSubKey("Software\\IcyWind");
            key?.SetValue("Path", findLeagueDialog.FileName.Replace("LeagueClient.exe", string.Empty));

            return findLeagueDialog.FileName.Replace("LeagueClient.exe", string.Empty);
        }
    }
}