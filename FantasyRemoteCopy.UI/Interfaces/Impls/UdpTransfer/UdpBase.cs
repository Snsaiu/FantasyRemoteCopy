using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;

public abstract class UdpBase : IDisposable
{
    protected UdpClient? UdpClient;

    protected virtual UdpClient CreateUdpClient() => new();

    public void Dispose()
    {
        UdpClient?.Close();
    }
}