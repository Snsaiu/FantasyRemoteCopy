using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.UdpTransfer;
namespace AirTransfer;

public sealed class LocalNetDeviceDiscovery : LocalNetDeviceDiscoveryBase
{
    public LocalNetDeviceDiscovery(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}