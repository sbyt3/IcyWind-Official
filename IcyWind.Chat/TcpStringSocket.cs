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
    public class TcpStringSocket
    {
        
        private TcpStringSocket(IPEndPoint hostIp)
        {
            
        }
    }
}
