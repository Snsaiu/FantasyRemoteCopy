using System.Text;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class UdpLoopReceiveBase<T> : UdpBase, IReceiveable<T>
{
    public bool Stop { get; set; }
    public async Task ReceiveAsync(Action<T> receiveCallBack)
    {
        UdpClient ??= CreateUdpClient();
        
        Stop = false;
        //var endPoint = new IPEndPoint(IPAddress.Any, ConstParams.INVITE_PORT);
        while (true)
        {
            if(Stop)
                return;
            var receivedData = await UdpClient.ReceiveAsync();
            var message = Encoding.UTF8.GetString(receivedData.Buffer);
            var model = JsonConvert.DeserializeObject<T>(message);
            if(model is null)
                continue;
            receiveCallBack?.Invoke(model);
        }

    }
}