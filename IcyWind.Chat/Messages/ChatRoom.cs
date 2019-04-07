using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IcyWind.Chat.Messages
{
    public class ChatRoom
    {
        public Jid.UserJid RoomJid { get; }

        private ChatClient ChatClient { get; }

        internal ChatRoom(ChatClient chatClient, Jid.UserJid roomJid)
        {
            ChatClient = chatClient;
            RoomJid = roomJid;
        }

        public void SendRoomMessage(string message)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var encodedXml = System.Web.HttpUtility.HtmlEncode(message);
            ChatClient.TcpClient.SendString($"<message from=\'{ChatClient.MainJid.RawJid}\' to=\'{RoomJid.RawJid}\' type=\'groupchat\'><body>{encodedXml}</body></message>");
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }
    }
}
