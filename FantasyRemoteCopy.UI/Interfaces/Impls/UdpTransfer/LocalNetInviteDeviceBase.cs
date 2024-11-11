using System.Net;
using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;

/// <summary>
/// 局域网设备邀请
/// </summary>
public abstract class LocalNetInviteDeviceBase : UdpSendBase<DeviceDiscoveryMessage>
{
    protected override UdpClient CreateUdpClient()=>new UdpClient(){EnableBroadcast = true};

    protected override IPEndPoint SetTarget(DeviceDiscoveryMessage invite)=>new IPEndPoint(IPAddress.Parse(invite.TargetFlag), ConstParams.INVITE_PORT);

}