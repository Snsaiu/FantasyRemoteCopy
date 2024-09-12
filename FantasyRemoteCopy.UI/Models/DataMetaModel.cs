namespace FantasyRemoteCopy.UI.Models;

/// <summary>
/// 数据的元数据信息
/// </summary>
public class DataMetaModel
{
    public string Guid { get; set; } = string.Empty;

    public long Size { get; set; }


    /// <summary>
    /// 目标ip，该字段是要发送到哪台设备上，对于接收者无需关心
    /// </summary>
    public string TargetIp { get; set; } = string.Empty;

    /// <summary>
    /// 传输的数据类型
    /// </summary>
    public DataType DataType { get; set; }

    /// <summary>
    /// 文件名,包含后缀
    /// </summary>
    public string FileNameWithExtension { get; set; } = string.Empty;

    /// <summary>
    /// 如果是文件，该字段记录文件的原始磁盘位置，接收者无需关心此字段
    /// </summary>
    public string SourcePosition { get; set; } = string.Empty;

    /// <summary>
    /// 是否发送完成，如果发送完成为true，否则为false
    /// </summary>
    public MetaState State { get; set; }
}


public enum DataType
{
    //Image,
    Text,
    File,
}


/// <summary>
/// 数据当前状态
/// </summary>
public enum MetaState
{
    Receiving,
    Received,
    Sending,
    Sended,

}