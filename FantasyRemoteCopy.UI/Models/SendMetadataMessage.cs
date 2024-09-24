using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendMetadataMessage : IName, ISendType,IFlag,ISize
{
    public SendMetadataMessage(string flag, string name, long size)
    {
        SendType = SendType.File;
        Name = name;
        Size = size;
        Flag = flag;
    }

    public SendMetadataMessage(string flag,long size)
    {
        SendType = SendType.Text;
        Flag = flag;
        Size = size;
    }

    public string Name { get; init; } = string.Empty;

    public long Size { get; init; }
    public SendType SendType { get; init; }
    public string Flag { get; init; }
}