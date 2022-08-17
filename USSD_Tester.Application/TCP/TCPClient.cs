using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using USSD.App.Settings;

namespace USSD.App.TCP
{
    public class TCPClient : ITCPClient
    {
        private readonly IUSSDSettings _settings;
        private readonly Socket _socket;
        public TCPClient(IUSSDSettings settings)
        {
            _settings = settings;
            IPHostEntry host = Dns.GetHostEntry(_settings.Address);
            IPAddress ipAddress = host.AddressList[0];


            // Create a TCP/IP  socket.
            _socket = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
        }
        public async Task<Socket> StartClient()
        {
            IPHostEntry host = Dns.GetHostEntry(_settings.Address);
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, int.Parse(_settings.Port));
            await _socket.ConnectAsync(remoteEP);
            return _socket;

        }
        public async Task<byte[]> ReceiveAsync()
        {
            throw new Exception();
        }
        public async Task<int> SendData(string message)
        {
            if (!message.EndsWith("\n"))
            {
                message = message + "\n";
            }
            byte[] msg = Encoding.ASCII.GetBytes(message);

            // Send the data through the socket.
            int bytesSent = await Task.Run(() => _socket.Send(msg));

            return bytesSent;
        }
        public async Task CloseClient()
        {
            await Task.Run(() => _socket.Shutdown(SocketShutdown.Both));
            await Task.Run(() => _socket.Close());
        }
    }
}
