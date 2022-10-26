using FantasyRemoteCopy.Core.Enums;

namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 设备邀请模型
/// </summary>
public class SendInviteModel
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string MasterName { get; set; }

    /// <summary>
    /// 设备名称
    /// </summary>
    public string DeviceName { get; set; }

    /// <summary>
    /// 设备昵称（用户自己取的）
    /// </summary>
    public string NickName{ get; set; }

    /// <summary>
    /// 设备类型
    /// </summary>
    public string DevicePlatform { get; set; }
}

