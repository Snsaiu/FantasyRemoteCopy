using FantasyRemoteCopy.UI.Extensions;

using Newtonsoft.Json;

using System.Net;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class UdpSendBase<T> : UdpBase, ISendeable<T>
{
    protected abstract IPEndPoint SetTarget(T message);
    public Task SendAsync(T message, CancellationToken cancellationToken)
    {
        UdpClient ??= CreateUdpClient();
        var json = JsonConvert.SerializeObject(message);
        var data = json.ToBytes();

        return UdpClient.SendAsync(data, data.Length, SetTarget(message));
    }
}