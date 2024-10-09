namespace FantasyRemoteCopy.UI.Models;

/// <summary>
///     针对发送的类型是文件类型
/// </summary>
/// <param name="flag"></param>
/// <param name="targetFlag"></param>
/// <param name="fileFullPath"></param>
public class SendFileModel(string flag, string targetFlag, string fileFullPath)
    : SendModelBase(flag, targetFlag, fileFullPath)
{
    public override long Size
    {
        get
        {
            if (string.IsNullOrEmpty(FileFullPath))
                return 0;
            if (!File.Exists(FileFullPath))
                return 0;
            FileInfo fileInfo = new(FileFullPath);
            return fileInfo.Length;
        }
    }
}