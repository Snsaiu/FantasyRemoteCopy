using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Consts;

using System.Net.Sockets;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class TcpBase<T> : ISendableWithProgress<T> where T : IFlag
{
    protected abstract Task SendProcessAsync(NetworkStream stream, T message, IProgress<double>? progress);

    protected abstract SendType GetSendType();

    public async Task SendAsync(T message, IProgress<double>? progress)
    {
        TcpClient client = new TcpClient();
        try
        {
            await client.ConnectAsync(message.Flag, ConstParams.TCP_PORT);
            NetworkStream stream = client.GetStream();
            SendType sendType = GetSendType();
            await stream.WriteAsync(new byte[] { (byte)sendType }, 0, 1);
            await SendProcessAsync(stream, message, progress);
        }
        finally
        {
            client.Close();
        }
    }
}