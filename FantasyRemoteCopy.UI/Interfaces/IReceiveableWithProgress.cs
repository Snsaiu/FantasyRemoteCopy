namespace FantasyRemoteCopy.UI.Interfaces;

public interface IReceiveableWithProgress : IListenable
{
    Task ReceiveAsync(Action<object> receivedCallBack, IProgress<double>? progress);

    Task IListenable.ReceiveAsync(System.Action<object> receivedCallBack)
    {
        return ReceiveAsync(receivedCallBack, null);
    }
}

public interface IReceiveableWithProgress<T> : IReceiveableWithProgress
{
    Task ReceiveAsync(Action<T> receivedCallBack, IProgress<double>? progress);

    Task IReceiveableWithProgress.ReceiveAsync(System.Action<object> receivedCallBack, IProgress<double>? progress)
    {
        return ReceiveAsync(receivedCallBack, progress);
    }
}