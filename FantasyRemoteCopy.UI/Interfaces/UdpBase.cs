using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class UdpBase:IDisposable
{
    protected UdpClient? UdpClient;
    
    protected virtual UdpClient CreateUdpClient() => new UdpClient();
    
    public void Dispose()
    {
        UdpClient?.Close();
    }
}