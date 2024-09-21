using System.Net;
using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// 设备发现
/// </summary>
public interface IDeviceDiscoveryable
{
    bool Stop { get; set; }
    Task DiscoverDevicesAsync(Action<object> discoveryCallBack);
}

public interface IDeviceDiscoveryable<T>:IDeviceDiscoveryable
{
    Task DiscoverDevicesAsync(Action<T> discoveryCallBack);

    Task IDeviceDiscoveryable.DiscoverDevicesAsync(Action<object> discoveryCallBack)
    {
        if(discoveryCallBack is Action<T> action)
            discoveryCallBack(action);
        else
            throw new NotImplementedException();
        return Task.CompletedTask;
    }
}

public abstract class LocalNetDeviceDiscoveryBase : IDeviceDiscoveryable<LocalNetDeviceDiscoveryReceiveMessage>
{
    public async Task DiscoverDevicesAsync(Action<LocalNetDeviceDiscoveryReceiveMessage> discoveryCallBack)
    {
        var udpClient = new UdpClient(ConstParams.INVITE_PORT);
        var endPoint = new IPEndPoint(IPAddress.Any, ConstParams.INVITE_PORT);
        while (true)
        {
            if(Stop)
                return;
            var receivedData = await udpClient.ReceiveAsync();
            var message = Encoding.UTF8.GetString(receivedData.Buffer);
            var model = JsonConvert.DeserializeObject<LocalNetDeviceDiscoveryReceiveMessage>(message);
            if(model is null)
                continue;
            discoveryCallBack?.Invoke(model);
        }
        
    }

    public bool Stop { get; set; }
}