using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using Newtonsoft.Json;

using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class TcpSendBase<T, P> : ISendableWithProgress<T, P> where T : IFlag,ISize,ITargetFlag where P : IProgressValue
{
    protected abstract Task SendProcessAsync(NetworkStream stream, T message, IProgress<P>? progress);

    protected virtual SendMetadataMessage GetMetaDataMessage(T message)
    {
        return new SendMetadataMessage(message.Flag,message.TargetFlag,message.Size);
    }

    protected async Task SendTextAsync(NetworkStream stream, string text,int? size=null)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(text);
        await stream.WriteAsync(messageBytes, 0, size ?? messageBytes.Length);
    }

    private Task SendMetadataTextAsync(NetworkStream stream, string text)
    {
        byte[] originalData = Encoding.UTF8.GetBytes(text);
        
        // 2. 获取原始长度并将其放到 byte[]
        int originalLength = originalData.Length; // 记录原始长度
        byte[] lengthBytes = BitConverter.GetBytes(originalLength);

        // 3. 创建一个 80 字节的数组，先拷贝长度信息，再拷贝原始数据
        byte[] buffer = new byte[80];
        Array.Copy(lengthBytes, buffer, lengthBytes.Length); // 将长度信息放到前 4 字节
        Array.Copy(originalData, 0, buffer, 4, originalData.Length); // 从第 5 字节开始放原始数据

        // 4. 填充剩余字节为 0x00（这已经自动完成，因为 buffer 初始为 0）
        return stream.WriteAsync(buffer, 0,buffer.Length );
    }

    private Task SendMetadataMessageAsync(NetworkStream stream, T message)
    {
        SendMetadataMessage metaData = GetMetaDataMessage(message);
        string? json = JsonConvert.SerializeObject(metaData);
        return json is null ? throw new NullReferenceException() : SendMetadataTextAsync(stream, json);
    }

    public async Task SendAsync(T message, IProgress<P>? progress)
    {
        TcpClient client = new TcpClient();
        try
        {
            await client.ConnectAsync(message.Flag, ConstParams.TCP_PORT);
            NetworkStream stream = client.GetStream();

            await SendMetadataMessageAsync(stream, message);

            await SendProcessAsync(stream, message, progress);
        }
        finally
        {
            client.Close();
        }
    }
}