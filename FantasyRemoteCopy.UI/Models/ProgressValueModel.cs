using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class ProgressValueModel(string flag, double progress) : IProgressValue, IFlag
{
    public double Progress { get; } = progress;
    public string Flag { get; } = flag;
}