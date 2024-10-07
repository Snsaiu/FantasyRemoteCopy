using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public abstract class SendModelBase(string flag, string targetFlag,string fileFullPath) : IFlag, ISize, ITargetFlag
{
    public string Flag { get; } = flag;
    public abstract long Size { get; protected set; }
    public string TargetFlag { get; } = targetFlag;
    
    public string FileFullPath { get; } = fileFullPath;
}