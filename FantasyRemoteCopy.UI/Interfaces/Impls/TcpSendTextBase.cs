using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// tcp发送文本基类
/// </summary>
public abstract class TcpSendTextBase : TcpSendBase<SendTextModel, ProgressValueModel>
{
    protected override async Task SendProcessAsync(NetworkStream stream, SendTextModel message, IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message.Text);
        await stream.WriteAsync(messageBytes, 0, (int)message.Size, cancellationToken);
        progress?.Report(new ProgressValueModel(message.Flag, message.TargetFlag, 1));
    }

}