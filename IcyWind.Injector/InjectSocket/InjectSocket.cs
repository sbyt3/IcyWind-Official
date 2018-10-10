using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Injector.InjectSocket
{
    public class InjectSocket
    {
        private Socket _listenSocket { get; }
        
        public delegate void SocketException(Exception ex);
        public event SocketException OnInjectSocketException;

        public InjectSocket(IPEndPoint socketAddressPort)
        {
            if (!Equals(socketAddressPort.Address, IPAddress.Loopback))
            {
                throw new NonLoopbackIPAddressException("You must use IPAddress.Loopback");
            }

            _listenSocket = new Socket(socketAddressPort.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _listenSocket.Bind(socketAddressPort);
                _listenSocket.Listen(100);

            }
            catch (Exception ex)
            {
                OnInjectSocketException?.Invoke(ex);
            }
        }
    }
}
