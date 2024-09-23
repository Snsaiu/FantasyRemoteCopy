namespace FantasyRemoteCopy.UI.Interfaces;

public interface IReceiveableWithProgress : IListenable
{
    Task ReceiveAsync(Action<object> receivedCallBack, IProgress<object>? progress);

    Task IListenable.ReceiveAsync(System.Action<object> receivedCallBack)
    {
        return ReceiveAsync(receivedCallBack, null);
    }
}

public interface IReceiveableWithProgress<T,P> : IReceiveableWithProgress where P : IProgressValue
{
    Task ReceiveAsync(Action<T> receivedCallBack, IProgress<P>? progress);

    Task IReceiveableWithProgress.ReceiveAsync(System.Action<object> receivedCallBack, IProgress<object>? progress)
    {
        return ReceiveAsync(receivedCallBack, progress);
    }
}