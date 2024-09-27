using Newtonsoft.Json;

using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class UdpLoopIListenBase<T> : UdpBase, IListenable<T>
{
    protected virtual Task OnCancelReceiveAsync() => Task.CompletedTask;

    public async Task ReceiveAsync(Action<T> receiveCallBack, CancellationToken cancellationToken)
    {
        UdpClient ??= CreateUdpClient();

        while (true)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var receivedData = await UdpClient.ReceiveAsync(cancellationToken);
                var message = Encoding.UTF8.GetString(receivedData.Buffer);
                var model = JsonConvert.DeserializeObject<T>(message);
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