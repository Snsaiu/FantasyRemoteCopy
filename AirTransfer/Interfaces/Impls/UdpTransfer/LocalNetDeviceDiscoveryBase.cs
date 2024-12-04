using System.Net.Sockets;

using AirTransfer.Models;

using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace AirTransfer.Interfaces.Impls.UdpTransfer;

/// <summary>
/// 设备发现
/// </summary>
public abstract class LocalNetDeviceDiscoveryBase(DeviceLocalIpBase localIpBase)
    : UdpLoopIListenBase<DeviceDiscoveryMessage>(localIpBase)
{
    protected override UdpClient CreateUdpClient()
    {
        return new(ConstParams.INVITE_PORT);
    }
}