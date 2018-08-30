using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcyWind.Core.Logic.Riot.com.riotgames.platform.serviceproxy;
using RtmpSharp.Messaging;

namespace IcyWind.Core.Logic.Riot
{
    public class LcdsRiotCalls
    {
        internal Task DoLcdsProxyCall(string method, string service, params string[] args)
        {
            var guid = Guid.NewGuid();
            return RiotCalls.InvokeAsync<object>("lcdsServiceProxy", "call", guid.ToString("D"), method, service, args);
        }

        internal Task<LcdsServiceProxyResponse> DoLcdsProxyCallWithResponse(string method, string service,
            string args)
        {
            var guid = Guid.NewGuid();
            RiotCalls.InvokeAsync<object>("lcdsServiceProxy", "call", guid.ToString("D"), method, service, args);
            var t = new Task<LcdsServiceProxyResponse>(() =>
            {
                LcdsServiceProxyResponse rtmpResponse = null;

                void Handler(object sender, MessageReceivedEventArgs eventArgs)
                {
                    if (!(eventArgs.Body is LcdsServiceProxyResponse response) ||
                        response.MessageId != guid.ToString("D")) return;
                    rtmpResponse = response;
                    RiotCalls.RiotConnection.MessageReceived -= Handler;
                }

                RiotCalls.RiotConnection.MessageReceived += Handler;

                while (rtmpResponse == null)
                {
                    Task.Delay(1000);
                }

                return rtmpResponse;
            });
            t.Start();
            return t;
        }

        internal Task<LcdsServiceProxyResponse[]> WithMoreThanOneResponce(string method, string service, int responces,
            string args)
        {
            var result = new List<LcdsServiceProxyResponse>();
            var guid = Guid.NewGuid();
            RiotCalls.InvokeAsync<object>("lcdsServiceProxy", "call", guid.ToString("D"), method, service, args);
            var t = new Task<LcdsServiceProxyResponse[]>(() =>
            {
                void Handler(object sender, MessageReceivedEventArgs eventArgs)
                {
                    if (!(eventArgs.Body is LcdsServiceProxyResponse response) ||
                        response.MessageId != guid.ToString("D")) return;
                    result.Add(response);
                    if (result.Count == responces)
                    {
                        RiotCalls.RiotConnection.MessageReceived -= Handler;
                    }
                }

                RiotCalls.RiotConnection.MessageReceived += Handler;

                while (result.Count != responces)
                {
                    Task.Delay(1000);
                }

                return result.ToArray();
            });
            t.Start();
            return t;
        }

        private RiotCalls RiotCalls { get; }
        public LcdsRiotCalls(RiotCalls calls)
        {
            RiotCalls = calls;
        }
    }
}
