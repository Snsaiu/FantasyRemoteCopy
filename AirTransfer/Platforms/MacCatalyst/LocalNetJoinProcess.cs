using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.UdpTransfer;


namespace AirTransfer;

public sealed class LocalNetJoinProcess(DeviceLocalIpBase localIpBase) : LocalNetJoinProcessBase(localIpBase)
{
}