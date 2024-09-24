using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class TransformResultModel<T>(string flag, SendType sendType, T result) : IFlag, ISendType
{
    public T Result { get; init; } = result;

    public string Flag { get; init; } = flag;
    public SendType SendType { get; init; } = sendType;
}