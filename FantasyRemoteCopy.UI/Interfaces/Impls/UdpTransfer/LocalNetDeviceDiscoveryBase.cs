using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;

/// <summary>
/// 设备发现
/// </summary>
public abstract class LocalNetDeviceDiscoveryBase(DeviceLocalIpBase localIpBase)
    : UdpLoopIListenBase<DeviceDiscoveryMessage>(localIpBase)
{
    protected override UdpClient CreateUdpClient() => new(ConstParams.INVITE_PORT);
}