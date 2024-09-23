using System.Net.Sockets;
using FantasyRemoteCopy.UI.Consts;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpBase<T>:ISendableWithProgress<T>  where T:IFlag
{
    protected abstract Task SendProcessAsync(NetworkStream stream, T message, IProgress<double>? progress);

    public async Task SendAsync(T message, IProgress<double>? progress)
    {
        TcpClient client = new TcpClient();
        try
        {
            await client.ConnectAsync(message.Flag, ConstParams.TCP_PORT);
            var stream = client.GetStream();
            await SendProcessAsync(stream,message, progress);
        }
        finally
        {
            client.Close();
        }
    }
}