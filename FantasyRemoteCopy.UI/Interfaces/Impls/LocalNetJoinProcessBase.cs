using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class LocalNetJoinProcessBase:UdpLoopReceiveBase<JoinMessageModel>
{
    protected override UdpClient CreateUdpClient() => new UdpClient(ConstParams.JOIN_PORT);

}