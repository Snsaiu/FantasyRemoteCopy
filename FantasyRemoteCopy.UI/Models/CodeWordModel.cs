using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Models;

public enum CodeWordType
{
    CheckingPort,
    CheckingPortCanNotUse,
    CheckingPortCanUse,
    CancelTransmission
}

public class CodeWordModel : IFlag, ITargetFlag, IPort, ISendType, ITaskGuid
{
    public static CodeWordModel Create(string taskId, CodeWordType wordType, string localIp, string targetIp, int port,
        SendType sendType) =>
        new CodeWordModel(taskId, wordType, localIp, targetIp, port, sendType, null);

    public static CodeWordModel Create(string taskId, CodeWordType wordModel, string localIp, string targetIp, int port,
        SendType sendType,
        CancellationTokenSource cancellationTokenSource) =>
        new CodeWordModel(taskId, wordModel, localIp, targetIp, port, sendType, cancellationTokenSource);


    public CodeWordModel(string taskGuid, CodeWordType type, string flag, string targetFlag, int port,
        SendType sendType,
        CancellationTokenSource? cancellationTokenSource)
    {
        Type = type;
        Flag = flag;
        TargetFlag = targetFlag;
        Port = port;
        SendType = sendType;
        TaskGuid = taskGuid;
        CancellationTokenSource = cancellationTokenSource;
    }

    public CodeWordType Type { get; set; }
    public string Flag { get; }
    public string TargetFlag { get; }
    public int Port { get; }

    [JsonIgnore] public CancellationTokenSource? CancellationTokenSource { get; }
    public SendType SendType { get; }
    public string TaskGuid { get; }
}