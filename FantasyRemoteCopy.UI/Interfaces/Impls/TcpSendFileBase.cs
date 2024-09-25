using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// tcp 发送文件基类
/// </summary>
public abstract class TcpSendFileBase : TcpSendBase<SendFileModel, ProgressValueModel>
{
    protected override SendMetadataMessage GetMetaDataMessage(SendFileModel message)
    {
        FileInfo fileInfo = new FileInfo(message.FileFullPath);

        return new SendMetadataMessage(message.Flag, message.TargetFlag, Path.GetFileName(message.FileFullPath), fileInfo.Length);
    }

    protected override async Task SendProcessAsync(NetworkStream stream, SendFileModel message, IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {
        FileInfo fileInfo = new FileInfo(message.FileFullPath);

        long totalBytes = fileInfo.Length;
        long bytesSent = 0;

        byte[] buffer = new byte[8192]; // 8KB 缓冲区
        int bytesRead;

        await using FileStream fs = new FileStream(message.FileFullPath, FileMode.Open, FileAccess.Read);
        while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
            bytesSent += bytesRead;

            // 计算并显示上传进度
            double p = (double)bytesSent / totalBytes;

            progress?.Report(new ProgressValueModel(message.Flag, message.TargetFlag, p));

            // Console.WriteLine($"上传进度: {progress:F2}%");
        }
    }
}