using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Tools;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;

public abstract class TcpLoopListenContentBase(FileSavePathBase fileSavePathBase)
    : TcpLoopListenerBase<TransformResultModel<string>, ProgressValueModel, string>
{
    protected readonly FileSavePathBase FileSavePathBase = fileSavePathBase;

    protected override async Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<TransformResultModel<string>> receivedCallBack,
        IProgress<ProgressValueModel>? progress, int port, CancellationToken cancellationToken)
    {
        if (message.SendType == SendType.Text)
        {
            string text = await ReceiveStringAsync(stream, message.Size);
            TransformResultModel<string> result = new TransformResultModel<string>(message.Flag, SendType.Text, text, port);
            receivedCallBack.Invoke(result);
        }
        else
        {
            byte[] buffer = new byte[8192]; // 8KB 缓冲区
            long fileSize = message.Size;
            string guidFolder = Guid.NewGuid().ToString("N");

            Directory.CreateDirectory(Path.Combine(FileSavePathBase.SaveLocation, guidFolder));

            string saveFullPath = Path.Combine(FileSavePathBase.SaveLocation, guidFolder, message.Name);
            long receivedBytes = 0;

            await using FileStream? fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write);
            int bytesRead;
            while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await fs.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                receivedBytes += bytesRead;

                // 计算并显示下载进度
                double p = (double)receivedBytes / fileSize;
                ProgressValueModel pModel = new ProgressValueModel(message.Flag, message.TargetFlag, p);
                progress?.Report(pModel);
            }

            //如果是需要解压的，那么要先解压，在删除
            if (message.IsCompress)
            {
                fs?.Close();
                fs?.Dispose();

                ZipHelper.ExtractToDirectory(saveFullPath, Path.Combine(FileSavePathBase.SaveLocation, guidFolder));
                File.Delete(saveFullPath);

                string fileName = Path.Combine(FileSavePathBase.SaveLocation, guidFolder, Path.GetFileNameWithoutExtension(saveFullPath));
                TransformResultModel<string> result = new TransformResultModel<string>(message.Flag, SendType.Folder, fileName, port);
                receivedCallBack.Invoke(result);
            }
            else
            {
                TransformResultModel<string> result = new TransformResultModel<string>(message.Flag, SendType.File, saveFullPath, port);
                receivedCallBack.Invoke(result);
            }

        }
    }
}