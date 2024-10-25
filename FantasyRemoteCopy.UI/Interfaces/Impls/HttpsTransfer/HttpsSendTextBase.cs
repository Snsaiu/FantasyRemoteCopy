using System.Text;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.HttpsTransfer;

public abstract class HttpsSendTextBase(ISendPortService sendPortService)
    : HttpsSendBase<SendTextModel, ProgressValueModel>(sendPortService)
{
    protected override async Task SendProcessAsync(HttpClient sender, SendTextModel message,
        IProgress<ProgressValueModel>? progress,
        CancellationToken cancellationToken)
    {
        var port = SendPortService.GetPort();
        // 获得对方的ip
        var url = $"https://{message.TargetFlag}:{port}/text";

        var content = new StringContent(message.Text, Encoding.UTF8, "text/plain");

        // 发送请求
        var response = await sender.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        progress?.Report(new ProgressValueModel(message.Flag, message.TargetFlag, 1));
    }
}