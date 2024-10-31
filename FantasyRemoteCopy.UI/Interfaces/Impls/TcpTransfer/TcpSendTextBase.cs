﻿using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;

/// <summary>
///     tcp发送文本基类
/// </summary>
public abstract class TcpSendTextBase : TcpSendBase<SendTextModel, ProgressValueModel>
{
    protected override async Task SendProcessAsync(NetworkStream sender, SendTextModel message,
        IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message.Text);
        await sender.WriteAsync(messageBytes, 0, (int)message.Size, cancellationToken);
        progress?.Report(new ProgressValueModel(message.Flag, message.TargetFlag, 1));
    }
    
}