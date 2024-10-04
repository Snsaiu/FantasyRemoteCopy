using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI;

public sealed class FileSavePath : FileSavePathBase
{
    // public string GetSaveLocation()
    // {
    //     var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FRCData");
    //     if (Directory.Exists(path) == false)
    //         Directory.CreateDirectory(path);
    //     return path;
    // }
}