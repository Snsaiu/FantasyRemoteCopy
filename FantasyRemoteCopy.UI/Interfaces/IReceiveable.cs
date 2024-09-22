namespace FantasyRemoteCopy.UI.Interfaces;

public interface IReceiveable
{
    bool Stop { get; set; }
    Task ReceiveAsync(Action<object> receiveCallBack);
}

public interface IReceiveable<T>:IReceiveable
{
    Task ReceiveAsync(Action<T> receiveCallBack);

    Task IReceiveable.ReceiveAsync(Action<object> receiveCallBack)
    {
        if(receiveCallBack is Action<T> action)
            receiveCallBack(action);
        else
            throw new NotImplementedException();
        return Task.CompletedTask;
    }
}