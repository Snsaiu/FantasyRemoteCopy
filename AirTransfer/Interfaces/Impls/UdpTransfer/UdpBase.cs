using System.Net.Sockets;

namespace AirTransfer.Interfaces.Impls.UdpTransfer;

public abstract class UdpBase : IDisposable
{
    protected UdpClient? UdpClient;

    protected virtual UdpClient CreateUdpClient() => new();

    public void Dispose()
    {
        UdpClient?.Close();
    }
}