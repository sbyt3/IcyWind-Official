using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game.map
{
    [RtmpSharp("com.riotgames.platform.game.map.GameMapEnabledDTO")]
    public class GameMapEnabledDTO : RiotRtmpObject 
    {
        [RtmpSharp("minPlayers")]
        public int MinPlayers { get; set; }

        [RtmpSharp("gameMapId")]
        public int GameMapId { get; set; }
    }
}
