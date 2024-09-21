using System.Net;
using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Extensions;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 局域网设备邀请
/// </summary>
public abstract class LocalNetInviteDeviceBase : IInviteable<LocalNetInviteMessage>
{
    public async Task InviteAsync(LocalNetInviteMessage invite)
    {
        var udpClient = new UdpClient();
        udpClient.EnableBroadcast = true;
        var json = JsonConvert.SerializeObject(invite);
        var data = json.ToBytes();
        var endPoint = new IPEndPoint(IPAddress.Broadcast, ConstParams.INVITE_PORT);
        await udpClient.SendAsync(data,data.Length, endPoint);
        udpClient.Close();
    }
}