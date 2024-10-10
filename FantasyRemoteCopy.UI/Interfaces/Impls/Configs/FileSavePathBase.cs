namespace FantasyRemoteCopy.UI.Interfaces.Impls.Configs;

public abstract class FileSavePathBase:IFileSavePath
{
    public virtual string SaveLocation => FileSystem.CacheDirectory;
}