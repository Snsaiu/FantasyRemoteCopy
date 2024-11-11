using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
namespace FantasyRemoteCopy.UI;

public sealed class LocalNetDeviceDiscovery(DeviceLocalIpBase localIpBase) :LocalNetDeviceDiscoveryBase(localIpBase)
{

}