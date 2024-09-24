using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpLoopListenContentBase : TcpLoopListenerBase<TransformResultModel<string>, ProgressValueModel, string>
{
    protected override async Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<TransformResultModel<string>> receivedCallBack,
        IProgress<ProgressValueModel>? progress)
    {

        if (message.SendType == SendType.Text)
        {
            string text = await ReceiveStringAsync(stream);

            TransformResultModel<string> result = new TransformResultModel<string>(message.Flag, SendType.Text, text);

            receivedCallBack?.Invoke(result);
        }
        else
        {
            byte[] buffer = new byte[8192]; // 8KB 缓冲区
            long fileSize = message.Size;
            string saveFullPath = Path.Combine(ConstParams.SaveFilePath(), message.Name);
            long receivedBytes = 0;

            await using FileStream fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
            {
                await fs.WriteAsync(buffer, 0, bytesRead);
                receivedBytes += bytesRead;

                // 计算并显示下载进度
                double p = (double)receivedBytes / fileSize * 100;
                ProgressValueModel pModel = new ProgressValueModel(message.Flag, p);
                progress?.Report(pModel);
            }

            TransformResultModel<string> result = new TransformResultModel<string>(message.Flag, SendType.File, saveFullPath);
            receivedCallBack?.Invoke(result);
        }
    }
}