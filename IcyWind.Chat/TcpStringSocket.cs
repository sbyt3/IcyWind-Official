using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Chat
{
    /// <summary>
    /// This class sets up a socket
    /// This socket connects to a given xmpp server and invokes certain events
    /// This event should return a bool, being true if the message is a full xmpp message
    /// </summary>
    internal class TcpStringSocket
    {
        public delegate void StringSend(string sendString);
        public event StringSend OnStringSend;


        private TcpClient _client;

        /// <summary>
        /// Ony used internally, Creates the TcpSocket
        /// </summary>
        /// <param name="hostIp"></param>
        internal TcpStringSocket(IPEndPoint hostIp)
        {
            //Create the TCP connection to the xmpp server
            //Use TCP NoDelay to improve conneciton speed
            _client = new TcpClient
            {
                NoDelay = true
            };
            _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            _client.Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, true);
            _client.Connect(hostIp);
        }

        internal int SendString(string stringToSend)
        {
            return _client.Client.Send(Encoding.UTF8.GetBytes(stringToSend));
        }
    }
}
