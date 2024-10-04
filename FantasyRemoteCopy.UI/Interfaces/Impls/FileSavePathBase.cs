namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class FileSavePathBase:IFileSavePath
{
    public virtual string SaveLocation => FileSystem.CacheDirectory;
}