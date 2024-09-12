using FantasyRemoteCopy.UI.Interfaces;
namespace FantasyRemoteCopy.UI;

public class AppDataFolderFileSaveLocation : IFileSaveLocation
{
    public string GetSaveLocation()
    {

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FRCData");
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        return path;
    }
}