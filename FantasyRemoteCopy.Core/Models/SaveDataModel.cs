using SQLite;

namespace FantasyRemoteCopy.Core.Models;

/// <summary>
/// 保存的数据
/// </summary>
public class SaveDataModel
{
    /// <summary>
    /// id
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public SaveDataType DataType { get; set; }

    public string SourceDeviceNickName { get; set; } = string.Empty;

    public string Guid { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;


}

public enum SaveDataType
{
    Txt,
    File,
    Image
}