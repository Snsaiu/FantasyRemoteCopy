using FantasyRemoteCopy.UI.Interfaces.Impls;
namespace FantasyRemoteCopy.UI;

public sealed class LocalNetDeviceDiscovery : LocalNetDeviceDiscoveryBase
{
    public LocalNetDeviceDiscovery(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}