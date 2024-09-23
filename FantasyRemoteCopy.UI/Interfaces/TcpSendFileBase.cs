using System.Net.Sockets;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// tcp 发送文件基类
/// </summary>
public abstract class TcpSendFileBase : TcpBase<SendFileModel>
{
    protected override async Task SendProcessAsync(NetworkStream stream, SendFileModel message, IProgress<double>? progress)
    {
        FileInfo fileInfo = new FileInfo(message.FileFullPath);
        long totalBytes = fileInfo.Length;
        long bytesSent = 0;

        byte[] buffer = new byte[8192]; // 8KB 缓冲区
        int bytesRead;

        using (FileStream fs = new FileStream(message.FileFullPath, FileMode.Open, FileAccess.Read))
        {
            while ((bytesRead = await fs.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await stream.WriteAsync(buffer, 0, bytesRead);
                bytesSent += bytesRead;

                // 计算并显示上传进度
                double p = (double)bytesSent / totalBytes * 100;
                progress?.Report(p);

                // Console.WriteLine($"上传进度: {progress:F2}%");
            }
        }
    }
}