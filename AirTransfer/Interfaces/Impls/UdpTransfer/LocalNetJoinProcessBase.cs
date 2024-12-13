using System.Net.Sockets;

using AirTransfer.Models;

using FantasyRemoteCopy.UI.Consts;

namespace AirTransfer.Interfaces.Impls.UdpTransfer;

public class LocalNetJoinProcessBase(DeviceLocalIpBase localIpBase) : UdpLoopIListenBase<JoinMessageModel>(localIpBase)
{
    protected override UdpClient CreateUdpClient()
    {
        return new(ConstParams.JOIN_PORT);
    }
}