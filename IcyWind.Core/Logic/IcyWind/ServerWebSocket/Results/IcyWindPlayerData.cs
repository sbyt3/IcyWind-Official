using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.IcyWind.ServerWebSocket.Results
{
    public class IcyWindPlayerData
    {
        public IcyWindMasteries PlayerMasteries { get; set; }

        public IcyWindRunes PlayerRunes { get; set; }

        public string Username { get; set; }

        public string Rank { get; set; }

        public int PlayerPoints { get; set; }
    }
}
