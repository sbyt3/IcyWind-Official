using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.IcyWind.ServerWebSocket.Results;
using Newtonsoft.Json;

namespace IcyWind.Core.Logic.IcyWind.ServerWebSocket
{
    public class IcyWindCalls
    {
        
        public IcyWindQueue JoinQueue()
        {
            var resultString = string.Empty;
            return JsonConvert.DeserializeObject<IcyWindQueue>(resultString);
        }

        public IcyWindPlayerData GetPlayerData(string username)
        {
            var resultString = string.Empty;
            return JsonConvert.DeserializeObject<IcyWindPlayerData>(resultString);
        }

        public IcyWindGameHistoryList GetGameHistory(string username)
        {
            var resultString = string.Empty;
            return JsonConvert.DeserializeObject<IcyWindGameHistoryList>(resultString);
        }

        public bool SetMasteries(IcyWindMasteries masteries)
        {
            return true;
        }

        public bool SetRunes(IcyWindRunes runes)
        {
            return true;
        }

        public int GetPlayersInQueue(int queueId)
        {
            return 0;
        }

        public bool AcceptGameQueue()
        {
            return true;
        }

        public bool DeclineGame()
        {
            return true;
        }

        public bool PickChampion(int champId)
        {
            return true;
        }
    }
}
