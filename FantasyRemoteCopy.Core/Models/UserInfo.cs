using SQLite;

namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfo
{

    [PrimaryKey,AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// 用户名（唯一编号）
    /// </summary>
    public string Name { get; set; }
}