using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class TransmissionTaskModel : CodeWordModel
{
    public TransmissionTaskModel(string taskId, CodeWordType type, string flag, string targetFlag, int port,
        SendType sendType, CancellationTokenSource? cancellationTokenSource) : base(taskId, type, flag, targetFlag,
        port, sendType, cancellationTokenSource)
    {
    }
}