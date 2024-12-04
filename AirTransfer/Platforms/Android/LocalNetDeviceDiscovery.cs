using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.UdpTransfer;

namespace AirTransfer;

public sealed class LocalNetDeviceDiscovery(DeviceLocalIpBase localIpBase) : LocalNetDeviceDiscoveryBase(localIpBase)
{
}