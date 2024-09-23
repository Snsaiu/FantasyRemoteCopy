using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// tcp发送文本基类
/// </summary>
public abstract class TcpSendTextBase : TcpBase<SendTextModel>
{
    protected override SendType GetSendType()
    {
        return SendType.Text;
    }
    protected override async Task SendProcessAsync(NetworkStream stream, SendTextModel message, IProgress<double>? progress)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message.Text);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }
}