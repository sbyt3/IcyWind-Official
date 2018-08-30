using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.IcyWind.ServerWebSocket.Results
{
    public class IcyWindQueue
    {
        /// <summary>
        /// Defines if the queue is Ranked or Normal
        /// </summary>
        public string QueueType { get; set; }

        /// <summary>
        /// Estimated queue time in Seconds
        /// </summary>
        public int QueueTime { get; set; }

        /// <summary>
        /// Defines the Map (SumRift)
        /// </summary>
        public string MapType { get; set; }
    }
}
