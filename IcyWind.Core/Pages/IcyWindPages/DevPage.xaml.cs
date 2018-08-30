using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using IcyWind.Chat;
using IcyWind.Core.Logic;
using IcyWind.Core.Logic.IcyWind;
using IcyWind.Core.Logic.Riot.Compression;
using IcyWind.Core.Logic.Riot.Lobby;
using Newtonsoft.Json;

namespace IcyWind.Core.Pages.IcyWindPages
{
    /// <summary>
    /// Interaction logic for DevPage.xaml
    /// </summary>
    public partial class DevPage : UserControl
    {
        public DevPage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var messageData = JsonConvert.DeserializeObject<PartyPhaseMessage>(
                Encoding.UTF8.GetString(Gzip.Decompress(Convert.FromBase64String("H4sIAAAAAAAA/+VYTU/jMBC98yuinKkU5zt7a0vFVgttRXtDCDnJQCMcuxs7sBXiv6+dNiH9wOwiUFbaXtJ6ZjzPL/bMq59PDMNMWEkFFOY3wz9Vv1dLzGGCc5Aj5vB7/3I2nk5u56OL0XBhVh4/SyhhnEq761jVSLLE+SpjdA4EEjEXWKjoZ2mSRgE4r7xN5N3ZXhxHPdv2cM/F4PVCP3B6gR/FyAl8cANUpdhGDZdYXDH2gWhextU6VNygP7mdjYc/ahtORAVVXGRcSIfrathonsYW+Oazda8goNM9AyuGQMgRm1ivqtytvFtLTVUVZO2aWL4iIEBZ7jDh0BhfTt/F5mmweR1jizTYoo6xBRpsQcfYHA02p2Nslgab9RFsoft54FwNOLdj4nwNNr9jbKEGW9gxNluDzf5SbNtvN9XzZttFkrIogIp+3UzGNIVfr7OaCRSQM5oBH6wPnJ5fGi9CeNMq1boIySBdyObX6kx7pDQNdbfnJMePHy9ziQOKuqXjOGVGxo0cJ9m9edR1U5gjy7FcPzpK4CxLHsZSNlBxSOTbHGPOs3sK6YzxTDGi4Eymk9EeipVcCNos0D202EdOMX/Itvne3VHvkYe05PWfgLMcRmm61lDn+shFked2Sd2bzO1z+nnU2VrqVkzKQoZspNtzge9Zdoj+M+IcLXHnxjlLjUBHWxhECHnOP0mbjb6KNldL21mB7xm9yOgDijTc+YEbRnLTdVrm0Fvkvb/l6uZUO5pAIV//YQOxjxPraYnVkPkX7/SN1H53qYPuUofdpY4+IfWOQKqlDWEJJjOC13DwJ7kWTwvZEsgiy+Eyk7JH6aDIkp89J2m/ghxnNKP3jWPgO35QXxUUOAU1eF2LM6mi2NN0JWTEtBTTuwGmKrqWdm2vuTxTm2uLzUkVRbljPytXJEuwAFUL+N4MBRSMkN37jjrwqrIdZG2ieLOohsyauhgLQWDAGBfH5x68OhzMHqtRHBNQKxuqy512gWrPrWTpI25J3sYJbywthd1rSpRZUvlqHyCdV9WozXsloytIPbSzoLpYDoAmy4MlxWp0RBXmQyivdXaT6aTecdXcZslhTB/lNmHFeobF0rZQ2Cj4l9/w94A44RIAAA=="))));
            UserInterfaceCore.ChangeMainPageView<ChampionSelectPage>(messageData);
        }

        private void GamePath_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(UserClient.GameDirectory);
        }


        private string NewPres()
        {
            return "<body>" +
                   "<statusMsg>Reported</statusMsg>" +
                   "<mapId/>" +
                   "<rankedLeagueName/>" +
                   "<skinVariant/>" +
                   $"<pty>{{\"partyId\":\"{StaticVars.ActiveClient.CurrentParty.Payload.CurrentParty.PartyId}\",\"queue\"\"\"+++Ifzed\":{420},\"summoners\":[{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.AcctId}]*$#^&(%^#@#$%#@$#FUCK#(>???}}</pty>" +
                   "<skinname/>" +
                   $"<profileIcon>{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.Summoner.ProfileIconId}</profileIcon>" +
                   "<rankedLeagueDivision/>" +
                   "<isObservable>ALL</isObservable>" +
                   "<gameQueueType>NORMAL</gameQueueType>" +
                   "<gameStatus>hosting_NORMAL</gameStatus>" +
                   $"<level>{StaticVars.ActiveClient.LoginDataPacket.AllSummonerData.SummonerLevelAndPoints.SummonerLevel}</level>" +
                   $"<queueId>{420}</queueId>" +
                   "<gameMode>CLASSIC</gameMode>" +
                   "</body>";

            // "<rankedLeagueQueue>RANKED_SOLO_5x5</rankedLeagueQueue>" +
            // "<rankedWins>3</rankedWins><" +
            // "championId/>" +
            // "<rankedLeagueTier>PROVISIONAL</rankedLeagueTier>" +
            // "<tier>UNRANKED</tier>" +
            // "<rankedLosses>6</rankedLosses>" +
        }

        private void CustomMes(object senger, RoutedEventArgs e)
        {
            StaticVars.ActiveClient.XmppClient.SetPresence(TextBox.Text, PresenceType.Available, PresenceShow.Chat);
        }

        private void Crash(object sender, RoutedEventArgs e)
        {
            StaticVars.ActiveClient.XmppClient.SetPresence(NewPres(), PresenceType.Available, PresenceShow.Chat);
        }
    }
}
