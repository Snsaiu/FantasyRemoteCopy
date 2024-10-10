using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
namespace FantasyRemoteCopy.UI;

public sealed class LocalNetDeviceDiscovery : LocalNetDeviceDiscoveryBase
{
    public LocalNetDeviceDiscovery(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}