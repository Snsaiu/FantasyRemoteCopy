using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;
using FantasyRemoteCopy.UI.Enums;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class TcpLoopListenContentBase : TcpLoopListenerBase<TransformResultModel<string>, ProgressValueModel, string>
{
    protected override async Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<TransformResultModel<string>> receivedCallBack,
        IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {

        if (message.SendType == SendType.Text)
        {
            var text = await ReceiveStringAsync(stream, message.Size);
            var result = new TransformResultModel<string>(message.Flag, SendType.Text, text);
            receivedCallBack.Invoke(result);
        }
        else
        {
            byte[] buffer = new byte[8192]; // 8KB 缓冲区
            long fileSize = message.Size;
            string saveFullPath = Path.Combine(ConstParams.SaveFilePath(), message.Name);
            long receivedBytes = 0;

            await using var fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer,cancellationToken)) > 0)
            {
                await fs.WriteAsync(buffer, 0, bytesRead,cancellationToken);
                receivedBytes += bytesRead;

                // 计算并显示下载进度
                double p = (double)receivedBytes / fileSize;
                var pModel = new ProgressValueModel(message.Flag, message.TargetFlag, p);
                progress?.Report(pModel);
            }

            var result = new TransformResultModel<string>(message.Flag, SendType.File, saveFullPath);
            receivedCallBack.Invoke(result);
        }
    }
}