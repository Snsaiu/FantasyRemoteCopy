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