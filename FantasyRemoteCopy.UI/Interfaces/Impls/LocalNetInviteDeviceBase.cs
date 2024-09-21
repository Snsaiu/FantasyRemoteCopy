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
public abstract class LocalNetInviteDeviceBase : IInviteable<LocalNetInviteMessage>,IDisposable
{

    private readonly UdpClient _udpClient;
    public LocalNetInviteDeviceBase()
    {
        _udpClient = new UdpClient(){EnableBroadcast = true};
    }
    public async Task InviteAsync(LocalNetInviteMessage invite)
    {
        var json = JsonConvert.SerializeObject(invite);
        var data = json.ToBytes();
        var endPoint = new IPEndPoint(IPAddress.Broadcast, ConstParams.INVITE_PORT);
        await _udpClient.SendAsync(data,data.Length, endPoint);
    }
    public void Dispose()
    {
        _udpClient.Close();
    }
}