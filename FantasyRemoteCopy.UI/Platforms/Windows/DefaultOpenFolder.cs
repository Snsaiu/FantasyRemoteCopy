using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public class DefaultOpenFolder : IOpenFolder
{
    public void OpenFolder(string path)
    {

        System.Diagnostics.Process.Start("Explorer.exe", path);
    }
}