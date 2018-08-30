using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.IcyWind.ServerWebSocket.Results
{
    //TODO: Finish this class
    public class IcyWindGameHistoryList
    {
        public List<IcyWindGameHistory> GameHistoryList { get; set; }
    }

    public class IcyWindGameHistory
    {
        public int GameLength { get; set; }
        public IcyWindKillHistory KillHistory { get; set; }
        public List<IcyWindPlayerGame> BlueTeamPlayers { get; set; }
        public List<IcyWindPlayerGame> PurpleTeamPlayers { get; set; }

        public List<IcyWindGameEvent> GameEvents { get; set; }

        /// <summary>
        /// Either Blue or Purple
        /// </summary>
        public string WinningTeam { get; set; }
    }

    public class IcyWindGameEvent
    {
        /// <summary>
        /// These are big kills (Dragon, Baron, Tower, Inhib)
        /// </summary>
        public string EventType { get; set; }

        public string EventTime { get; set; }
    }

    public class IcyWindPlayerGame
    {
        /// <summary>
        /// This is the gold a player has updated every 30 seconds
        /// </summary>
        public List<int> PlayerGold { get; set; }

        public IcyWindPlayerData PlayerDataBeforeGame { get; set; }

        public List<IcyWindKillHistory> KillHistory { get; set; }
    }

    public class IcyWindKillHistory
    {
        public int Time { get; set; }

        public string PlayerKilled { get; set; }

        public string KillingPlayer { get; set; }
    }
}
