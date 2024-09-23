using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class TransformResultModel<T>(string flag, T result) : IFlag
{
    public T Result { get; init; } = result;

    public string Flag { get; init; } = flag;
}