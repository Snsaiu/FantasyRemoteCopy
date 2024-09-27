using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public sealed class DefaultOpenFolder : IOpenFolder
{
    public void OpenFolder(string path)
    {
        System.Diagnostics.Process.Start("Explorer.exe", path);
    }
}