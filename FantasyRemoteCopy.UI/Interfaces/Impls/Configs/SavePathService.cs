namespace FantasyRemoteCopy.UI.Interfaces.Impls.Configs
{
    public class SavePathService : ISavePathService
    {
        public string? GetPath() => Preferences.Default.Get<string>("savePath", string.Empty);

        public void SavePath(string path) => Preferences.Default.Set<string>("savePath", path);
    }
}
