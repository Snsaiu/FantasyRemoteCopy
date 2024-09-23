using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;

using Newtonsoft.Json;

using System.Net.Sockets;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpSendBase<T> : ISendableWithProgress<T> where T : IFlag
{
    protected abstract Task SendProcessAsync(NetworkStream stream, T message, IProgress<double>? progress);

    protected virtual SendMetadataMessage GetMetaDataMessage(T message)
    {
        return new SendMetadataMessage();
    }

    protected async Task SendTextAsync(NetworkStream stream, string text)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(text);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }

    private Task SendMetadataMessageAsync(NetworkStream stream, T message)
    {
        SendMetadataMessage metaData = GetMetaDataMessage(message);
        string? json = JsonConvert.SerializeObject(metaData);
        return json is null ? throw new NullReferenceException() : SendTextAsync(stream, json);
    }

    public async Task SendAsync(T message, IProgress<double>? progress)
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