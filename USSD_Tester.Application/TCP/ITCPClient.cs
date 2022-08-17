using System.Net.Sockets;
using System.Threading.Tasks;

namespace USSD.App.TCP
{
    public interface ITCPClient
    {
        Task CloseClient();
        Task<byte[]> ReceiveAsync();
        Task<int> SendData(string message);
        Task<Socket> StartClient();
    }
}