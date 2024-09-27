using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 设备发现
/// </summary>
public abstract class LocalNetDeviceDiscoveryBase : UdpLoopIListenBase<DeviceDiscoveryMessage>
{
    protected override UdpClient CreateUdpClient() => new(ConstParams.INVITE_PORT);
}