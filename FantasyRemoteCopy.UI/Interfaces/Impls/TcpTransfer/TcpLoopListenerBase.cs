﻿using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;

public abstract class LoopListenerBase<T, P, R> : IReceiveableWithProgress<T, P>
    where T : TransformResultModel<R> where P : IProgressValue
{
    protected virtual Task OnCancelReceiveAsync()
    {
        return Task.CompletedTask;
    }

    public abstract Task ReceiveAsync(Action<T> receivedCallBack, IPAddress address, int port, IProgress<P>? progress,
        CancellationToken cancellationToken);
}

public abstract class TcpLoopListenerBase<T, P, R> : LoopListenerBase<T, P, R>
    where T : TransformResultModel<R> where P : IProgressValue
{
    public override async Task ReceiveAsync(Action<T> receivedCallBack, IPAddress address, int port,
        IProgress<P>? progress, CancellationToken cancellationToken)
    {
        using TcpListener listener = new TcpListener(IPAddress.Any, port);

        listener.Start();

        while (true)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                TcpClient client = await listener.AcceptTcpClientAsync(cancellationToken);
                _ = HandleClientAsync(client, port, receivedCallBack, progress, cancellationToken); // 处理客户端连接
            }
            catch (OperationCanceledException)
            {
                listener?.Stop();
                await OnCancelReceiveAsync();
                break;
            }
        }
    }

    protected async Task<string> ReceiveStringAsync(NetworkStream stream, long length)
    {
        byte[] buffer = new byte[length];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    private async Task<string> ReceiveMetadataStringAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[200];
        _ = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

        // 2. 读取前 4 字节的原始长度信息
        int originalLength = BitConverter.ToInt32(buffer, 0);

        // 3. 根据原始长度读取有效数据并转回字符串
        byte[] originalData = new byte[originalLength];
        Array.Copy(buffer, 4, originalData, 0, originalLength); // 跳过前 4 字节长度信息

        return Encoding.UTF8.GetString(originalData);
    }

    protected abstract Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<T> receivedCallBack, IProgress<P>? progress, int port, CancellationToken cancellationToken);

    private async Task HandleClientAsync(TcpClient client, int port, Action<T> receivedCallBack, IProgress<P>? progress,
        CancellationToken cancellationToken)
    {
        NetworkStream stream = client.GetStream();
        string metaString = await ReceiveMetadataStringAsync(stream, cancellationToken);
        SendMetadataMessage? metaMessage = JsonConvert.DeserializeObject<SendMetadataMessage>(metaString);
        if (metaMessage is null)
        {
            throw new NullReferenceException();
        }

        await HandleReceiveAsync(stream, metaMessage, receivedCallBack, progress, port, cancellationToken);
    }
}