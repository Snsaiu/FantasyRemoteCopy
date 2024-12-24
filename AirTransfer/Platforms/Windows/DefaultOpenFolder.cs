using AirTransfer.Interfaces;

namespace AirTransfer;

public sealed class DefaultOpenFolder : IOpenFolder
{
    public void OpenFolder(string path)
    {
        System.Diagnostics.Process.Start("Explorer.exe", path);
    }
}