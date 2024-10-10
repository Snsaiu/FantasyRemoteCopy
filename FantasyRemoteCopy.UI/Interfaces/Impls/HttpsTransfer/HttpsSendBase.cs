﻿using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.HttpsTransfer;

public abstract class HttpsSendBase<T, P> : SendBase<T, P, HttpClient>
    where T : IFlag, ISize, ITargetFlag where P : IProgressValue
{
    public sealed override Task SendAsync(T message, IProgress<P>? progress, CancellationToken cancellationToken)
    {
        var handler = new HttpClientHandler
        {
            // 忽略证书验证错误（用于自签名证书的测试场景）
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        using var client = new HttpClient(handler);
        return SendProcessAsync(client, message, progress, cancellationToken);
    }
}

public abstract class HttpsSendTextBase : HttpsSendBase<SendTextModel, ProgressValueModel>
{
    protected override Task SendProcessAsync(HttpClient sender, SendTextModel message,
        IProgress<ProgressValueModel>? progress,
        CancellationToken cancellationToken)
    {
        // 获得对方的ip


    }
}