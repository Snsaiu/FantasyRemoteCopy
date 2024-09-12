namespace FantasyRemoteCopy.UI.Models;

public class FileDataModel
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileNameWithExtension { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public byte[] ContentBytes { get; set; } = [];
}