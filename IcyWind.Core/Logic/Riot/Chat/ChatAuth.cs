using IcyWind.Core.Logic.Data;
using IcyWind.Core.Logic.IcyWind;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IcyWind.Chat;

namespace IcyWind.Core.Logic.Riot.Chat
{
    public static class ChatAuth
    {
        public static async Task ConnectToChat(RegionData regionData, UserClient client, string rsoToken)
        {
            //TODO: check that the servers are the same ip, if not pick one with lowest RTT
            var servers = Dns.GetHostAddresses(regionData.Servers.Chat.ChatHost);
            client.Players = new List<ChatPlayerItem>();
            var chat = new ChatClient(
                new IPEndPoint(servers.First(), regionData.Servers.Chat.ChatPort));

            chat.GetChatPresence += () => chat.PresenceAsString(client.GetPresence(), PresenceType.Available, PresenceShow.Chat);
            chat.OnRosterItemRecieved += client.OnRostRecieve;
            chat.OnPlayerPresenceRecieved += client.OnPresRecieve;
            chat.OnMobilePresenceRecieved += client.OnMobilePresRecieve;
            chat.OnMessageRecieved += client.OnMessage;
            await chat.Connect("pvp.net");
            client.XmppClient = chat;
        }
    }
}
