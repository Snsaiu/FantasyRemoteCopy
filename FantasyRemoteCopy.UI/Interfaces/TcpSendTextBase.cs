using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// tcp发送文本基类
/// </summary>
public abstract class TcpSendTextBase : TcpSendBase<SendTextModel>
{
    protected override Task SendProcessAsync(NetworkStream stream, SendTextModel message, IProgress<double>? progress)
    {
        return SendTextAsync(stream, message.Text);
    }
}