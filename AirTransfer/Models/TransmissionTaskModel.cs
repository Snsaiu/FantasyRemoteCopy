using AirTransfer.Enums;
using AirTransfer.Interfaces;

using FantasyRemoteCopy.UI.Interfaces;

using Newtonsoft.Json;

namespace AirTransfer.Models;

public class TransmissionTaskModel : IFlag, ITargetFlag, IPort, ISendType, ITaskGuid
{
    public static TransmissionTaskModel Create(string taskId, string localIp, string targetIp,
        int port,
        SendType sendType)
    {
        return new TransmissionTaskModel(taskId, localIp, targetIp, port, sendType, null);
    }

    public static TransmissionTaskModel Create(string taskId, string localIp, string targetIp,
        int port,
        SendType sendType,
        CancellationTokenSource cancellationTokenSource)
    {
        return new TransmissionTaskModel(taskId, localIp, targetIp, port, sendType, cancellationTokenSource);
    }

    public TransmissionTaskModel(string taskGuid, string flag, string targetFlag, int port,
        SendType sendType,
        CancellationTokenSource? cancellationTokenSource)
    {
        Flag = flag;
        TargetFlag = targetFlag;
        Port = port;
        SendType = sendType;
        TaskGuid = taskGuid;
        CancellationTokenSource = cancellationTokenSource;
    }


    public string Flag { get; }
    public string TargetFlag { get; }
    public int Port { get; }

    [JsonIgnore] public CancellationTokenSource? CancellationTokenSource { get; }
    public SendType SendType { get; }
    public string TaskGuid { get; }
}