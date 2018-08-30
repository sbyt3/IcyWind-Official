using System;
using Newtonsoft.Json;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.serviceproxy
{
    [RtmpSharp("com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse")]
    public class LcdsServiceProxyResponse : RiotRtmpObject
    {
        [RtmpSharp("payload")]
        public string Payload { get; set; }

        [RtmpSharp("status")]
        public string Status { get; set; }

        [RtmpSharp("messageId")]
        public string MessageId { get; set; }

        [RtmpSharp("methodName")]
        public string MethodName { get; set; }

        [RtmpSharp("serviceName")]
        public string ServiceName { get; set; }

        [RtmpSharp("compressedPayload")]
        public bool CompressedPayload { get; set; }
    }
}
