using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RtmpSharp;
using RtmpSharp.IO.AMF3;

namespace IcyWind.Core.Logic.Riot.com.riotgames.platform.broadcast
{
    [RtmpSharp("com.riotgames.platform.broadcast.BroadcastNotification")]
    public class BroadcastNotification : RiotRtmpObject, IExternalizable
    {
        public ArrayList broadcastMessages { get; set; }

        public string Json { get; set; }

        public void ReadExternal(IDataInput input)
        {
            Json = input.ReadUtf((int)input.ReadUInt32());
            Dictionary<string, object> deserializedJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(Json);

            Type classType = typeof(BroadcastNotification);
            foreach (KeyValuePair<string, object> keyPair in deserializedJson)
            {
                var f = classType.GetProperty(keyPair.Key);
                f.SetValue(this, keyPair.Value);
            }
        }

        public void WriteExternal(IDataOutput output)
        {
            var bytes = Encoding.UTF8.GetBytes(Json);

            output.WriteInt32(bytes.Length);
            output.WriteBytes(bytes);
        }
    }
}
