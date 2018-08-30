using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.game
{
    [RtmpSharp("com.riotgames.platform.game.PlayerCredentialsDto")]
    public class PlayerCredentialsDTO : RiotRtmpObject
    {
        [RtmpSharp("encryptionKey")]
        public string EncryptionKey { get; set; }

        [RtmpSharp("gameId")]
        public long GameId { get; set; }

        [RtmpSharp("serverIp")]
        public string ServerIp { get; set; }

        [RtmpSharp("lastSelectedSkinIndex")]
        public long LastSelectedSkinIndex { get; set; }

        [RtmpSharp("observer")]
        public bool Observer { get; set; }

        [RtmpSharp("summonerId")]
        public long SummonerId { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("observerServerIp")]
        public string ObserverServerIp { get; set; }

        [RtmpSharp("dataVersion")]
        public long DataVersion { get; set; }

        [RtmpSharp("handshakeToken")]
        public string HandshakeToken { get; set; }

        [RtmpSharp("playerId")]
        public long PlayerId { get; set; }

        [RtmpSharp("serverPort")]
        public long ServerPort { get; set; }

        [RtmpSharp("observerServerPort")]
        public long ObserverServerPort { get; set; }

        [RtmpSharp("summonerName")]
        public string SummonerName { get; set; }

        [RtmpSharp("observerEncryptionKey")]
        public string ObserverEncryptionKey { get; set; }

        [RtmpSharp("championId")]
        public long ChampionId { get; set; }
    }
}
