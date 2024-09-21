using System.Net;
using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Extensions;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces;


/// <summary>
/// 设备邀请
/// </summary>
public interface IInviteable
{
    /// <summary>
    /// 发送邀请数据
    /// </summary>
    /// <param name="invite">邀请数据/param>
    /// <returns></returns>
    Task InviteAsync(object invite);
}

public interface IInviteable<TInvite> : IInviteable
{ 
    Task InviteAsync(TInvite invite);

    Task IInviteable.InviteAsync(object invite)=>this.InviteAsync(invite);
}

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