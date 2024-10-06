using Android.Content;
using Android.Webkit;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public class OpenFileProvider:IOpenFileable
{
    public void OpenFile(string filename)
    {
        Launcher.Default.OpenAsync(new OpenFileRequest("select", new ReadOnlyFile(filename)));
    }
}