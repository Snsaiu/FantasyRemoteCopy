using System.Net.Sockets;
using AirTransfer.Consts;
using AirTransfer.Models;

namespace AirTransfer.Interfaces.Impls.UdpTransfer;

public class LocalNetJoinProcessBase(DeviceLocalIpBase localIpBase) : UdpLoopIListenBase<JoinMessageModel>(localIpBase)
{
    protected override UdpClient CreateUdpClient()
    {
        return new(ConstParams.JOIN_PORT);
    }
}