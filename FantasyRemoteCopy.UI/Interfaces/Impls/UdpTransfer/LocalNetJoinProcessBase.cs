using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;

public class LocalNetJoinProcessBase(DeviceLocalIpBase localIpBase) : UdpLoopIListenBase<JoinMessageModel>(localIpBase)
{
    protected override UdpClient CreateUdpClient() => new(ConstParams.JOIN_PORT);
}