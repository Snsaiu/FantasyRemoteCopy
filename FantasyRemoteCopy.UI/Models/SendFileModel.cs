using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public class SendFileModel(string flag, string fileFullPath) : IFlag
{
    public string Flag { get; init; } = flag;

    public string FileFullPath { get; init; } = fileFullPath;
}