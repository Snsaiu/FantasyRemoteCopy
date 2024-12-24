using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.UdpTransfer;
namespace AirTransfer;

public sealed class LocalNetJoinProcess : LocalNetJoinProcessBase
{
    public LocalNetJoinProcess(DeviceLocalIpBase localIpBase) : base(localIpBase)
    {
    }
}