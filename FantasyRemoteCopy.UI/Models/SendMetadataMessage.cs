using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendMetadataMessage : IName, ISendType
{
    public SendMetadataMessage(string name, long size)
    {
        SendType = SendType.File;
        Name = name;
        Size = size;
    }

    public SendMetadataMessage()
    {
        SendType = SendType.Text;
    }

    public string Name { get; init; } = string.Empty;

    public long Size { get; init; }
    public SendType SendType { get; init; }
}