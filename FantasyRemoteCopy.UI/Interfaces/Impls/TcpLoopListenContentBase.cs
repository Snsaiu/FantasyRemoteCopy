using System.Net.Sockets;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Tools;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class TcpLoopListenContentBase(FileSavePathBase fileSavePathBase)
    : TcpLoopListenerBase<TransformResultModel<string>, ProgressValueModel, string>
{
    protected readonly FileSavePathBase FileSavePathBase = fileSavePathBase;

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
            var buffer = new byte[8192]; // 8KB 缓冲区
            var fileSize = message.Size;
            var guidFolder = Guid.NewGuid().ToString("N");
            
            Directory.CreateDirectory(Path.Combine( FileSavePathBase.SaveLocation,guidFolder));
            
            var saveFullPath = Path.Combine(FileSavePathBase.SaveLocation,guidFolder, message.Name);
            long receivedBytes = 0;

            await using var fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fs.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                receivedBytes += bytesRead;

                // 计算并显示下载进度
                var p = (double)receivedBytes / fileSize;
                var pModel = new ProgressValueModel(message.Flag, message.TargetFlag, p);
                progress?.Report(pModel);
            }

            //如果是需要解压的，那么要先解压，在删除
            if (message.IsCompress)
            {
                fs?.Close();
                fs?.Dispose();

                ZipHelper.ExtractToDirectory(saveFullPath, Path.Combine( FileSavePathBase.SaveLocation,guidFolder));
                File.Delete(saveFullPath);

                var fileName = Path.Combine(FileSavePathBase.SaveLocation,guidFolder, Path.GetFileNameWithoutExtension(saveFullPath));
                var result = new TransformResultModel<string>(message.Flag, SendType.Folder, fileName);
                receivedCallBack.Invoke(result);
            }
            else
            {
                var result = new TransformResultModel<string>(message.Flag, SendType.File, saveFullPath);
                receivedCallBack.Invoke(result);
            }
  
        }
    }
}