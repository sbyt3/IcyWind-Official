using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.systemstate
{
    [RtmpSharp("com.riotgames.platform.systemstate.ReplaySystemStates")]
    public class ReplaySystemStates : RiotRtmpObject
    {
        [RtmpSharp("matchHistoryEnabled")]
        public bool MatchHistoryEnabled { get; set; }

        [RtmpSharp("getReplaysTabEnabled")]
        public bool GetReplaysTabEnabled { get; set; }

        [RtmpSharp("backpatchingEnabled")]
        public bool BackpatchingEnabled { get; set; }

        [RtmpSharp("replayServiceAddress")]
        public string ReplayServiceAddress { get; set; }

        [RtmpSharp("replayServiceEnabled")]
        public bool ReplayServiceEnabled { get; set; }

        [RtmpSharp("endOfGameEnabled")]
        public bool EndOfGameEnabled { get; set; }
    }
}
