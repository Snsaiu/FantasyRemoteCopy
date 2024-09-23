namespace FantasyRemoteCopy.UI.Interfaces;


/// <summary>
/// 可发送数据
/// </summary>
public interface ISendeable
{
    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="message">数据</param>
    /// <returns></returns>
    Task SendAsync(object message);
}

public interface ISendeable<TMessage> : ISendeable
{
    Task SendAsync(TMessage message);

    Task ISendeable.SendAsync(object message)
    {
        return SendAsync(message);
    }
}