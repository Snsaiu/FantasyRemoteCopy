namespace FantasyRemoteCopy.Core.Models;

public class FileDataModel
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileNameWithExtension { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public byte[] ContentBytes { get; set; }
}