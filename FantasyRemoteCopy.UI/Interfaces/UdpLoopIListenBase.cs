using Newtonsoft.Json;

using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;

public abstract class UdpLoopIListenBase<T> : UdpBase, IListenable<T>
{
    public bool Stop { get; set; }
    public async Task ReceiveAsync(Action<T> receiveCallBack)
    {
        UdpClient ??= CreateUdpClient();

        Stop = false;
        //var endPoint = new IPEndPoint(IPAddress.Any, ConstParams.INVITE_PORT);
        while (true)
        {
            if (Stop)
                return;
            System.Net.Sockets.UdpReceiveResult receivedData = await UdpClient.ReceiveAsync();
            string message = Encoding.UTF8.GetString(receivedData.Buffer);
            T? model = JsonConvert.DeserializeObject<T>(message);
            if (model is null)
                continue;
            receiveCallBack?.Invoke(model);
        }

    }
}