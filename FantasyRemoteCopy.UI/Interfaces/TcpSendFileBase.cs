using FantasyRemoteCopy.UI.Models;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// tcp 发送文件基类
/// </summary>
public abstract class TcpSendFileBase : TcpSendBase<SendFileModel>
{
    protected override SendMetadataMessage GetMetaDataMessage(SendFileModel message)
    {
        FileInfo fileInfo = new FileInfo(message.FileFullPath);

        return new SendMetadataMessage(message.Flag, Path.GetFileName(message.FileFullPath), fileInfo.Length);
    }

    protected override async Task SendProcessAsync(NetworkStream stream, SendFileModel message, IProgress<double>? progress)
    {
        var fileInfo = new FileInfo(message.FileFullPath);

        var totalBytes = fileInfo.Length;
        long bytesSent = 0;

        var buffer = new byte[8192]; // 8KB 缓冲区
        int bytesRead;

        await using var fs = new FileStream(message.FileFullPath, FileMode.Open, FileAccess.Read);
        while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
            bytesSent += bytesRead;

            // 计算并显示上传进度
            var p = (double)bytesSent / totalBytes * 100;
            progress?.Report(p);

            // Console.WriteLine($"上传进度: {progress:F2}%");
        }
    }
}