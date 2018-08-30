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

namespace IcyWind.Core.Controls
{
    /// <summary>
    /// Interaction logic for EndOfGamePlayer.xaml
    /// </summary>
    public partial class EndOfGamePlayer : UserControl
    {
        private double summonerID;
        private double gameID;
        private string summonerName;
        private bool sameTeam;

        public EndOfGamePlayer(double summonerID, double gameId, string summonerName, bool sameTeam)
        {
            InitializeComponent();
            this.summonerID = summonerID;
            this.gameID = gameId;
            this.summonerName = summonerName;
            this.sameTeam = sameTeam;
        }
    }
}
