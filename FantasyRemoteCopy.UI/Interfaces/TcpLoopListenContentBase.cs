using System.Net.Sockets;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpLoopListenContentBase : TcpLoopListenerBase<TransformResultModel<string>,ProgressValueModel,string>
{
    protected override async Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<TransformResultModel<string>> receivedCallBack,
        IProgress<ProgressValueModel>? progress)
    {

        if (message.SendType == SendType.Text)
        {
            var text = await this.ReceiveStringAsync(stream);
            
            var result = new TransformResultModel<string>(message.Flag, text);
            
            receivedCallBack?.Invoke(result);
        }
        else
        {
            byte[] buffer = new byte[8192]; // 8KB 缓冲区
            var fileSize = message.Size;
            var saveFullPath = Path.Combine(ConstParams.SaveFilePath(), message.Name);
            long receivedBytes = 0;

            await using FileStream fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
            {
                await fs.WriteAsync(buffer, 0, bytesRead);
                receivedBytes += bytesRead;

                // 计算并显示下载进度
                var p = (double)receivedBytes / fileSize * 100;
                var pModel = new ProgressValueModel(message.Flag, p);
                progress?.Report(pModel);
            }

            var result = new TransformResultModel<string>(message.Flag, saveFullPath);
            receivedCallBack?.Invoke(result);
        }
    }
}