namespace FantasyRemoteCopy.Core.Impls;

public class AppDataFolderFileSaveLocation:IFileSaveLocation
{
    public string GetSaveLocation()
    {
        var path= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FRCData");
        if(Directory.Exists(path)==false)
            Directory.CreateDirectory(path);
        return path;
    }
}