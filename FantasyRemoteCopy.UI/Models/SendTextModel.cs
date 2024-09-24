using System.Text;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendTextModel(string flag, string text) : IFlag,ISize
{
    public string Flag { get; init; } = flag;

    public string Text { get; init; } = text;
    public long Size { get; } = Encoding.UTF8.GetByteCount(text);
}