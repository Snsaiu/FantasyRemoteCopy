using AirTransfer.Interfaces;

namespace AirTransfer;

public class OpenFileProvider:IOpenFileable
{
    public void OpenFile(string filename)
    {
         Launcher.OpenAsync(filename);
    }
}