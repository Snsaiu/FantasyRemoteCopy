using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public abstract class DeskTopFileSavePathBase: FileSavePathBase,IChangePathable
{
    private readonly ISavePathService savePathService;

    public DeskTopFileSavePathBase(ISavePathService savePathService)
    {
        this.savePathService=savePathService;
    }
    public override string SaveLocation
    {
        get
        {
            var path = savePathService.GetPath();
            if (string.IsNullOrEmpty(path))
                return base.SaveLocation;
            return path;
        }
    }

    void IChangePathable.ChangedPath(string path) => savePathService.SavePath(path);
}