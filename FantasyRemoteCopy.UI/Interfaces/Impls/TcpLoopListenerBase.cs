using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class TcpLoopListenerBase<T, P, R> : IReceiveableWithProgress<T, P>
    where T : TransformResultModel<R> where P : IProgressValue
{
    public bool Stop { get; set; }

    public async Task ReceiveAsync(Action<T> receivedCallBack, IProgress<P>? progress)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, ConstParams.TCP_PORT);
        listener.Start();

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();

            _ = HandleClientAsync(client, receivedCallBack, progress); // 处理客户端连接
        }
    }


    protected async Task<string> ReceiveStringAsync(NetworkStream stream, long length)
    {
        byte[] buffer = new byte[length];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    private  async Task<string> ReceiveMetadataStringAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[80];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        // 2. 读取前 4 字节的原始长度信息
        int originalLength = BitConverter.ToInt32(buffer, 0);

        // 3. 根据原始长度读取有效数据并转回字符串
        byte[] originalData = new byte[originalLength];
        Array.Copy(buffer, 4, originalData, 0, originalLength); // 跳过前 4 字节长度信息

        return Encoding.UTF8.GetString(originalData);
    }

    protected abstract Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<T> receivedCallBack, IProgress<P>? progress);

    private async Task HandleClientAsync(TcpClient client, Action<T> receivedCallBack, IProgress<P>? progress)
    {
        var stream = client.GetStream();

        var metaString = await ReceiveMetadataStringAsync(stream);

        var metaMessage = JsonConvert.DeserializeObject<SendMetadataMessage>(metaString);
        if (metaMessage is null)
        {
            throw new NullReferenceException();
        }

        await HandleReceiveAsync(stream, metaMessage, receivedCallBack, progress);
    }
}