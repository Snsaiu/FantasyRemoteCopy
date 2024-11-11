using System.Net;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;


public abstract class LocalNetJoinRequestBase:UdpSendBase<JoinMessageModel>
{
    protected override IPEndPoint SetTarget(JoinMessageModel message) => new(IPAddress.Parse(message.SendTarget), ConstParams.JOIN_PORT);
}