using System.Net;
using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

/// <summary>
/// 设备发现
/// </summary>
public abstract class LocalNetDeviceDiscoveryBase : IDeviceDiscoveryable<LocalNetInviteMessage>,IDisposable
{
    private readonly UdpClient _udpClient;
    public LocalNetDeviceDiscoveryBase()
    {
         _udpClient = new UdpClient(ConstParams.INVITE_PORT);
    }
    public async Task DiscoverDevicesAsync(Action<LocalNetInviteMessage> discoveryCallBack)
    {
        Stop = false;
       //var endPoint = new IPEndPoint(IPAddress.Any, ConstParams.INVITE_PORT);
        while (true)
        {
            if(Stop)
                return;
            var receivedData = await _udpClient.ReceiveAsync();
            var message = Encoding.UTF8.GetString(receivedData.Buffer);
            var model = JsonConvert.DeserializeObject<LocalNetInviteMessage>(message);
            if(model is null)
                continue;
            discoveryCallBack?.Invoke(model);
        }
        
    }

    public bool Stop { get; set; }
    public void Dispose()
    {
        _udpClient.Close();
    }
}