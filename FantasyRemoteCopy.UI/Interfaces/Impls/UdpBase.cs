using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class UdpBase : IDisposable
{
    protected UdpClient? UdpClient;

    protected virtual UdpClient CreateUdpClient()
    {
        return new UdpClient();
    }

    public void Dispose()
    {
        UdpClient?.Close();
    }
}