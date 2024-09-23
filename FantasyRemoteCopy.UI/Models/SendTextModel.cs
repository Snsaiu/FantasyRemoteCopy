using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendTextModel(string flag, string text) : IFlag
{
    public string Flag { get; init; } = flag;

    public string Text { get; init; } = text;
}