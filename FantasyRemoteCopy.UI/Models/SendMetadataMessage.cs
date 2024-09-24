using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Models;

public class SendMetadataMessage : IName, ISendType,IFlag,ISize,ITargetFlag
{
    public SendMetadataMessage(string flag,string targetFlag, string name, long size)
    {
        SendType = SendType.File;
        Name = name;
        Size = size;
        Flag = flag;
        TargetFlag= targetFlag;
    }

    public SendMetadataMessage(string flag,string targetFlag,long size)
    {
        SendType = SendType.Text;
        Flag = flag;
        Size = size;
        TargetFlag= targetFlag;
    }

    [JsonConstructor]
    public SendMetadataMessage(string name, string flag,string targetFlag, SendType sendType, long size)
    {
        SendType = sendType;
        Name = name;
        Flag = flag;
        Size = size;
        TargetFlag= targetFlag;
    }

    public string Name { get; init; } = string.Empty;

    public long Size { get; init; }
    public SendType SendType { get; init; }
    public string Flag { get; init; }
    public string TargetFlag { get; init; }
}