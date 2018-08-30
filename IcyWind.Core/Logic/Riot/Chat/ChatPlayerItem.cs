using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.Riot.Chat
{
    public class ChatPlayerItem
    {
        public string JidAsString { get; set; }

        public string Username { get; set; }

        public int ProfileIcon { get; set; }

        public int Level { get; set; }

        public int Wins { get; set; }

        public int RankedWins { get; set; }

        public int Leaves { get; set; }

        public string LeagueTier { get; set; }

        public string LeagueDivision { get; set; }

        public string LeagueName { get; set; }

        public string GameStatus { get; set; }

        public long Timestamp { get; set; }

        public bool Busy { get; set; }

        public string Champion { get; set; }

        public string Status { get; set; }

        public string RawPresence { get; set; }

        public string Group { get; set; }

        public bool IsOnline { get; set; }

        public bool IsAway { get; set; }

        public bool Mobile { get; set; }

        internal bool HasGottenNickname { get; set; }

        public List<KeyValuePair<string, string>> Messages { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
