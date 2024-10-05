using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI;

public sealed class FileSavePath : FileSavePathBase,IChangePathable
{
    private readonly ISavePathService savePathService;

    public FileSavePath(ISavePathService savePathService)
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