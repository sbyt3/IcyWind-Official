using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    public class ChatRoom
    {
        public Jid RoomJid { get; }

        private ChatClient _chatClient { get; }

        internal ChatRoom(ChatClient chatClient, Jid roomJid)
        {
            _chatClient = chatClient;
            RoomJid = roomJid;
        }

        public void SendRoomMessage(string message)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            var encodedXml = System.Web.HttpUtility.HtmlEncode(message);
            var messageSend =
                Encoding.UTF8.GetBytes(
                    $"<message from=\'{_chatClient.MainJid.RawJid}\' to=\'{RoomJid.RawJid}\' type=\'groupchat\'><body>{encodedXml}</body></message>");
            _chatClient.SslStream.Write(messageSend);
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        }
    }
}
