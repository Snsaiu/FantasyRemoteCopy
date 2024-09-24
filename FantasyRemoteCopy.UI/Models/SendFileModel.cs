using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendFileModel(string flag, string fileFullPath) : IFlag,ISize
{
    public string Flag { get; init; } = flag;

    public string FileFullPath { get; init; } = fileFullPath;

    public long Size
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