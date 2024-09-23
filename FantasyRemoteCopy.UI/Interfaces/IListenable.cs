namespace FantasyRemoteCopy.UI.Interfaces;

public interface IListenable
{
    bool Stop { get; set; }
    Task ReceiveAsync(Action<object> receivedCallBack);
}

public interface IListenable<T> : IListenable
{
    Task ReceiveAsync(Action<T> receivedCallBack);

    Task IListenable.ReceiveAsync(Action<object> receivedCallBack)
    {
        if (receivedCallBack is Action<T> action)
            receivedCallBack(action);
        else
            throw new NotImplementedException();
        return Task.CompletedTask;
    }


}