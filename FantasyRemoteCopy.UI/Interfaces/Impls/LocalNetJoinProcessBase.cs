using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class LocalNetJoinProcessBase : UdpLoopIListenBase<JoinMessageModel>
{
    protected override UdpClient CreateUdpClient() => new(ConstParams.JOIN_PORT);
}