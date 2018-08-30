using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.IcyWind.ServerWebSocket.Results
{
    public class IcyWindMasteries
    {
        public List<IcyWindMasteryBook> BookPages { get; set; }
    }

    public class IcyWindMasteryBook
    {
        public int MasteryId { get; set; }

        public int PointsInMastery { get; set; }

        public string MasteryName { get; set; }
    }
}
