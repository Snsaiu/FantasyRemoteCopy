
using FantasyRemoteCopy.Core.Enums;

namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 传输模型内容
/// </summary>
public class TransformData
{
    /// <summary>
    /// 数据内容
    /// </summary>
    public byte[] Data { get; set; }
    
    /// <summary>
    /// 数据类型
    /// </summary>
    public DataType Type { get; set; }
    
    /// <summary>
    /// 目标ip
    /// </summary>
    public string TargetIp { get; set; }

    /// <summary>
    /// 目标端口号
    /// </summary>
    public string Port { get; set; }

}

