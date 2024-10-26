using System.Net.NetworkInformation;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <inheritdoc />
public class PortChecker:IPortCheckable
{
    public Task<bool> IsPortInUse(int port)
    {
        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();

        return Task.FromResult(tcpConnections.All(c => c.LocalEndPoint.Port != port));
    }
}