namespace FantasyRemoteCopy.Core.Impls;

public class AppDataFolderFileSaveLocation:IFileSaveLocation
{
    public string GetSaveLocation()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            );
    }
}