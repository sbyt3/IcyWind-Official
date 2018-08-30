using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.matchmaking
{
    [RtmpSharp("com.riotgames.platform.matchmaking.QueueThrottleDTO")]
    public class QueueThrottleDTO :RiotRtmpObject
    {
        [RtmpSharp("queueThrottles")]
        public object QueueThrottles { get; set; }
    }
}
