using System.Net;
using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 设备发现
/// </summary>
public abstract class LocalNetDeviceDiscoveryBase : UdpLoopReceiveBase<DeviceDiscoveryMessage>
{
    protected override UdpClient CreateUdpClient() => new UdpClient(ConstParams.INVITE_PORT);
}