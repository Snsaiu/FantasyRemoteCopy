using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public sealed class AppDataFolderFileSaveLocation : IFileSaveLocation
{
    public string GetSaveLocation()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FRCData");
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        return path;
    }
}