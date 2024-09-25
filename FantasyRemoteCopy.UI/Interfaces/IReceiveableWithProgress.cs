namespace FantasyRemoteCopy.UI.Interfaces;

public interface IReceiveableWithProgress : IListenable
{
    Task ReceiveAsync(Action<object> receivedCallBack, IProgress<object>? progress, CancellationToken cancellationToken);

    Task IListenable.ReceiveAsync(System.Action<object> receivedCallBack, CancellationToken cancellationToken)
    {
        return ReceiveAsync(receivedCallBack, null, cancellationToken);
    }
}

public interface IReceiveableWithProgress<T, P> : IReceiveableWithProgress where P : IProgressValue
{
    Task ReceiveAsync(Action<T> receivedCallBack, IProgress<P>? progress, CancellationToken cancellationToken);

    Task IReceiveableWithProgress.ReceiveAsync(System.Action<object> receivedCallBack, IProgress<object>? progress, CancellationToken cancellationToken)
    {
        return ReceiveAsync(receivedCallBack, progress, cancellationToken);
    }
}