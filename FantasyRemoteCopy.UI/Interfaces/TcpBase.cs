namespace FantasyRemoteCopy.UI.Interfaces;

public class TcpBase<T>:ISendeable<T>,IDisposable
{
    
    public Task SendAsync(T message)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}