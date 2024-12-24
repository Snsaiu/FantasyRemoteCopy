using AirTransfer.Interfaces;

namespace AirTransfer;

public class OpenFileProvider : IOpenFileable
{
    public void OpenFile(string filename)
    {
        Launcher.Default.OpenAsync(new OpenFileRequest("select", new ReadOnlyFile(filename)));
    }
}