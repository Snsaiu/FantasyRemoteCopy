namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 数据的元数据信息
/// </summary>
public class DataMetaModel
{
    public string Guid { get; set; }

    public long Size { get; set; }

    /// <summary>
    /// 是否发送完成，如果发送完成为true，否则为false
    /// </summary>
    public bool Sended { get; set; } = false;
}
