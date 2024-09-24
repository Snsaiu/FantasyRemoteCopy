namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// 发送数据并且带有回调进度
/// </summary>
public interface ISendableWithProgress : ISendeable
{
    Task SendAsync(object message, IProgress<object>? progress);

    Task ISendeable.SendAsync(object message)
    {
        return SendAsync(message, null);
    }
}

public interface ISendableWithProgress<T, P> : ISendableWithProgress where P : IProgressValue
{
    Task SendAsync(T message, IProgress<P>? progress);

    Task ISendableWithProgress.SendAsync(object message, IProgress<object>? progress)
    {
        return SendAsync(message, progress);
    }
}