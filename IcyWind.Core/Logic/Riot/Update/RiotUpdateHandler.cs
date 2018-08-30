using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.IcyWind;
using zlib;

namespace IcyWind.Core.Logic.Riot.Update
{

    //http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/releaselisting_NA
    //http://l3cdn.riotgames.com/releases/live/solutions/lol_game_client_sln/releases/releaselisting_NA
    //403 below (Next one)
    //http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/0.0.0.123/tag
    //http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/0.0.0.123/solutionmanifest
    //This goes in Status
    //https://status.leagueoflegends.com/shards/na/synopsis
    //This is what we need
    //http://l3cdn.riotgames.com/releases/live/projects/league_client/releases/0.0.0.153/patchmanifests/0.0.0.151.patchmanifest
    //Another 403 below
    //http://l3cdn.riotgames.com/releases/live/projects/league_client/releases/0.0.0.153/tag
    //http://l3cdn.riotgames.com/releases/live/projects/league_client_en_us/releases/0.0.0.123/patchmanifests/0.0.0.121.patchmanifest
    //Another 403
    //http://l3cdn.riotgames.com/releases/live/projects/league_client_en_us/releases/0.0.0.123/tag
    //Unknown file. Looks like what needs updates
    //http://l3cdn.riotgames.com/releases/live/projects/league_client/releases/0.0.0.153/packages/patches/0.0.0.151/packagemanifest
    //https://status.leagueoflegends.com/shards/na/synopsis
    public static class RiotUpdateHandler
    {
        public static WebClient Client { get; } = new WebClient();

        public static string GetLeagueClientSlnReleaseListing()
        {
            return Client.DownloadString("http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/releaselisting_NA");
        }

        public static void GetLatestSystemYaml()
        {
            if (!Directory.Exists(Path.Combine(StaticVars.IcyWindLocation, "temp")))
            {
                Directory.CreateDirectory(Path.Combine(StaticVars.IcyWindLocation, "temp"));
            }
            if (!Directory.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system")))
            {
                Directory.CreateDirectory(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system"));
            }
            if (File.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml")))
            {
                File.Delete(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml"));
            }
            using (var client = new WebClient())
            {
                var LatestGCSln = client
                    .DownloadString(
                        "http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/releaselisting_NA")
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None).First();
                var LatestSolutionMan = client
                    .DownloadString(
                        $"http://l3cdn.riotgames.com/releases/live/solutions/league_client_sln/releases/{LatestGCSln}/solutionmanifest")
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                //LatestSolutionMan
                var pos = Array.IndexOf(LatestSolutionMan, "league_client");
                client.DownloadFile(
                    $"http://l3cdn.riotgames.com/releases/live/projects/league_client/releases/{LatestSolutionMan[pos + 1]}/files/system.yaml.compressed",
                    Path.Combine(StaticVars.IcyWindLocation, "temp", "system.yaml.compressed"));
                DecompressFile(Path.Combine(StaticVars.IcyWindLocation, "temp", "system.yaml.compressed"),
                    Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "system.yaml"));
                File.Delete(Path.Combine(StaticVars.IcyWindLocation, "temp", "system.yaml.compressed"));
            }
            Directory.Delete(Path.Combine(StaticVars.IcyWindLocation, "temp"), true);
        }

        public static void GetLatestSystemYamlPbe()
        {
            if (!Directory.Exists(Path.Combine(StaticVars.IcyWindLocation, "temp")))
            {
                Directory.CreateDirectory(Path.Combine(StaticVars.IcyWindLocation, "temp"));
            }
            if (!Directory.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system")))
            {
                Directory.CreateDirectory(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system"));
            }
            if (File.Exists(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "pbe.system.yaml")))
            {
                File.Delete(Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "pbe.system.yaml"));
            }
            using (var client = new WebClient())
            {
                var LatestGCSln = client
                    .DownloadString(
                        "http://l3cdn.riotgames.com/releases/pbe/solutions/league_client_sln/releases/releaselisting_PBE")
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None).First();
                var LatestSolutionMan = client
                    .DownloadString(
                        $"http://l3cdn.riotgames.com/releases/pbe/solutions/league_client_sln/releases/{LatestGCSln}/solutionmanifest")
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                //LatestSolutionMan
                var pos = Array.IndexOf(LatestSolutionMan, "league_client");
                client.DownloadFile(
                    $"http://l3cdn.riotgames.com/releases/pbe/projects/league_client/releases/{LatestSolutionMan[pos + 1]}/files/system.yaml.compressed",
                    Path.Combine(StaticVars.IcyWindLocation, "temp", "pbe.system.yaml.compressed"));
                DecompressFile(Path.Combine(StaticVars.IcyWindLocation, "temp", "pbe.system.yaml.compressed"),
                    Path.Combine(StaticVars.IcyWindLocation, "IcyWindAssets", "system", "pbe.system.yaml"));
                File.Delete(Path.Combine(StaticVars.IcyWindLocation, "temp", "pbe.system.yaml.compressed"));
            }
            Directory.Delete(Path.Combine(StaticVars.IcyWindLocation, "temp"));
        }

        public static void DecompressFile(string inFile, string outFile)
        {
            int data;
            const int stopByte = -1;
            var outFileStream = new FileStream(outFile, FileMode.Create);
            var inZStream = new ZInputStream(File.Open(inFile, FileMode.Open, FileAccess.Read));

            while (stopByte != (data = inZStream.Read()))
            {
                var databyte = (byte)data;
                outFileStream.WriteByte(databyte);
            }

            inZStream.Close();
            outFileStream.Close();
        }
    }
}
