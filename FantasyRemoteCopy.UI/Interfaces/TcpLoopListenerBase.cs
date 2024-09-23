using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using Newtonsoft.Json;

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;


public abstract class TcpLoopListenFileBase : TcpLoopListenerBase<string>
{

}

public abstract class TcpLoopListenerBase<T> : IReceiveableWithProgress<T>
{
    public bool Stop { get; set; }
    public async Task ReceiveAsync(Action<T> receivedCallBack, IProgress<double>? progress)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, ConstParams.TCP_PORT);
        listener.Start();

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("客户端已连接。");

            _ = HandleClientAsync(client, receivedCallBack, progress); // 处理客户端连接
        }
    }


    private async Task<string> ReceiveStringAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[8192];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    protected abstract Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message, Action<T> receivedCallBack, IProgress<double>? progress);

    private async Task HandleClientAsync(TcpClient client, Action<T> receivedCallBack, IProgress<double>? progress)
    {
        NetworkStream stream = client.GetStream();

        string metaString = await ReceiveStringAsync(stream);

        SendMetadataMessage? metaMessage = JsonConvert.DeserializeObject<SendMetadataMessage>(metaString);
        if (metaMessage is null)
        {
            throw new NullReferenceException();
        }
        await HandleReceiveAsync(stream, metaMessage, receivedCallBack, progress);

    }
}