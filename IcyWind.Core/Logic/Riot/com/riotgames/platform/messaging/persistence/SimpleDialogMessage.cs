using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.messaging.persistence
{
    [RtmpSharp("com.riotgames.platform.messaging.persistence.SimpleDialogMessage")]
    public class SimpleDialogMessage : RiotRtmpObject
    {
        [RtmpSharp("titleCode")]
        public string TitleCode { get; set; }

        [RtmpSharp("accountId")]
        public long AccountId { get; set; }

        [RtmpSharp("dataVersion")]
        public int DataVersion { get; set; }

        [RtmpSharp("msgId")]
        public string MsgId { get; set; }

        [RtmpSharp("params")]
        public string[] Params { get; set; }

        [RtmpSharp("type")]
        public string Type { get; set; }

        [RtmpSharp("futureData")]
        public object FutureData { get; set; }

        [RtmpSharp("bodyCode")]
        public string BodyCode { get; set; }
    }
}
