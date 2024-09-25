using Newtonsoft.Json;

using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class UdpLoopIListenBase<T> : UdpBase, IListenable<T>
{
    public bool Stop { get; set; }

    protected virtual Task OnCancelReceiveAsync()
    {
        return Task.CompletedTask;
    }

    public async Task ReceiveAsync(Action<T> receiveCallBack, CancellationToken cancellationToken)
    {
        UdpClient ??= CreateUdpClient();

        while (true)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                System.Net.Sockets.UdpReceiveResult receivedData = await UdpClient.ReceiveAsync(cancellationToken);
                string message = Encoding.UTF8.GetString(receivedData.Buffer);
                T? model = JsonConvert.DeserializeObject<T>(message);
                if (model is null)
                    continue;
                receiveCallBack?.Invoke(model);

            }
            catch (OperationCanceledException)
            {
                await OnCancelReceiveAsync();
            }
        }

    }
}