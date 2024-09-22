using System.Net;
using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;


public abstract class LocalNetJoinRequestBase:UdpSendBase<JoinMessageModel>
{
    protected override IPEndPoint SetTarget(JoinMessageModel message)
    {
        return new IPEndPoint(IPAddress.Parse(message.SendTarget), ConstParams.JOIN_PORT);
    }
}