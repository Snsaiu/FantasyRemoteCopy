using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using Newtonsoft.Json;

using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class TcpSendBase<T, P> : ISendableWithProgress<T, P> where T : IFlag, ISize, ITargetFlag where P : IProgressValue
{
    protected abstract Task SendProcessAsync(NetworkStream stream, T message, IProgress<P>? progress, CancellationToken cancellationToken);

    protected virtual SendMetadataMessage GetMetaDataMessage(T message) => new(message.Flag, message.TargetFlag, message.Size);

    protected  Task SendTextAsync(NetworkStream stream, string text, CancellationToken cancellationToken, int? size = null)
    {
        var messageBytes = Encoding.UTF8.GetBytes(text);
        return stream.WriteAsync(messageBytes, 0, size ?? messageBytes.Length, cancellationToken);
    }

    private Task SendMetadataTextAsync(NetworkStream stream, string text, CancellationToken cancellationToken)
    {
        var originalData = Encoding.UTF8.GetBytes(text);

        // 2. 获取原始长度并将其放到 byte[]
        var originalLength = originalData.Length; // 记录原始长度
        var lengthBytes = BitConverter.GetBytes(originalLength);

        // 3. 创建一个 80 字节的数组，先拷贝长度信息，再拷贝原始数据
        var buffer = new byte[200];
        Array.Copy(lengthBytes, buffer, lengthBytes.Length); // 将长度信息放到前 4 字节
        Array.Copy(originalData, 0, buffer, 4, originalData.Length); // 从第 5 字节开始放原始数据

        // 4. 填充剩余字节为 0x00（这已经自动完成，因为 buffer 初始为 0）
        return stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
    }

    private Task SendMetadataMessageAsync(NetworkStream stream, T message, CancellationToken cancellationToken)
    {
        var metaData = GetMetaDataMessage(message);
        var json = JsonConvert.SerializeObject(metaData);
        return json is null ? throw new NullReferenceException() : SendMetadataTextAsync(stream, json, cancellationToken);
    }

    public async Task SendAsync(T message, IProgress<P>? progress, CancellationToken cancellationToken)
    {
        var client = new TcpClient();
        try
        {
            await client.ConnectAsync(message.TargetFlag, ConstParams.TCP_PORT, cancellationToken);

            var stream = client.GetStream();
            await SendMetadataMessageAsync(stream, message, cancellationToken);
            await SendProcessAsync(stream, message, progress, cancellationToken);
        }
        finally
        {
            client.Close();
        }
    }
}