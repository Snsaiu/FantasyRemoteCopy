namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// 发送数据并且带有回调进度
/// </summary>
public interface ISendableWithProgress : ISendeable
{
    Task SendAsync(object message, IProgress<double>? progress);

    Task ISendeable.SendAsync(object message)=>SendAsync(message, null);
}