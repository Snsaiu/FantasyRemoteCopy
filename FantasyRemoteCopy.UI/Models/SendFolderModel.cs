namespace FantasyRemoteCopy.UI.Models;

public class SendFolderModel(string flag, string targetFlag, string fullPath)
    : SendModelBase(flag, targetFlag, fullPath)
{
    public override long Size { get; protected set; }
    /// <summary>
    /// 设置文件夹压缩后的大小
    /// </summary>
    /// <param name="size">压缩后的文件大小</param>
    public void SetCompressSize(long size)
    {
        Size = size;
    }
}