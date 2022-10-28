
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
    public TransformType Type { get; set; }
    
    /// <summary>
    /// 目标ip
    /// </summary>
    public string TargetIp { get; set; }

    /// <summary>
    /// 目标设备名称
    /// </summary>
    public string TargetDeviceNickName { get; set; }

    /// <summary>
    /// 目标端口号
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// 发送数据时候的唯一编号
    /// </summary>
    public string DataGuid{ get; set; }

}

