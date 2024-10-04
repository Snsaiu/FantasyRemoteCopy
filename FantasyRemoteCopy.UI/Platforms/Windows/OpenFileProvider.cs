namespace FantasyRemoteCopy.UI;

public class OpenFileProvider:IOpenFileable
{
    public void OpenFile(string filename)
    {
         Launcher.OpenAsync(model.Content);
    }
}