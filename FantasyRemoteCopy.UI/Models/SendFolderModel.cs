namespace FantasyRemoteCopy.UI.Models;

public class SendFolderModel(string flag, string targetFlag, string fullPath)
    : SendModelBase(flag, targetFlag, fullPath)
{
    public override long Size => throw new NotImplementedException();
}