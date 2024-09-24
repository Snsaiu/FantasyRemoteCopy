using FantasyRemoteCopy.UI.Extensions;

using Newtonsoft.Json;

using System.Net;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class UdpSendBase<T> : UdpBase, ISendeable<T>, IDisposable
{
    protected abstract IPEndPoint SetTarget(T message);
    public Task SendAsync(T message)
    {
        UdpClient ??= CreateUdpClient();

        string json = JsonConvert.SerializeObject(message);
        byte[] data = json.ToBytes();

        return UdpClient.SendAsync(data, data.Length, SetTarget(message));
    }





}