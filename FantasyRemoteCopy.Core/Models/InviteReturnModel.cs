namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 邀请返回模型
/// </summary>
public class InviteReturnModel
{
    public string Ip { get; set; } = string.Empty;

    public bool CanConnect { get; set; }
}