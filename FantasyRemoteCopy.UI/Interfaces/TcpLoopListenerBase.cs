using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpLoopListenerBase<T,P,R> : IReceiveableWithProgress<T,P> where T:TransformResultModel<R> where P:IProgressValue 
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


    protected async Task<string> ReceiveStringAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[8192];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer, 0, bytesRead);
    }

    protected abstract Task HandleReceiveAsync(NetworkStream stream, SendMetadataMessage message,
        Action<T> receivedCallBack, IProgress<P>? progress);

    private async Task HandleClientAsync(TcpClient client, Action<T> receivedCallBack, IProgress<P>? progress)
    {
        var stream = client.GetStream();

        var metaString = await ReceiveStringAsync(stream);

        var metaMessage = JsonConvert.DeserializeObject<SendMetadataMessage>(metaString);
        if (metaMessage is null)
        {
            throw new NullReferenceException();
        }

        await HandleReceiveAsync(stream, metaMessage, receivedCallBack, progress);
    }
}