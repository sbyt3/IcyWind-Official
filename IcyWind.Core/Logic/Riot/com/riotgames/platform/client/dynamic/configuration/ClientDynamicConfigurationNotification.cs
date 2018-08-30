using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RtmpSharp;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.client.dynamic.configuration
{
    [RtmpSharp("com.riotgames.platform.client.dynamic.configuration.ClientDynamicConfigurationNotification")]
    public class ClientDynamicConfigurationNotification : RiotRtmpObject
    {
        [RtmpSharp("configs")]
        public string Configs { get; set; }

        [RtmpSharp("delta")]
        public bool Delta { get; set; }
    }
}
