using Android.Content;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public sealed class DefaultOpenFolder : IOpenFolder
{
    public void OpenFolder(string path)
    {
        var context = Android.App.Application.Context;
        var uri = FileProvider.GetUriForFile(context, $"{context.PackageName}.fileprovider", new Java.IO.File(path));
        var intent = new Intent(Intent.ActionView);
        intent.SetDataAndType(uri, "*/*");
        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
        intent.AddFlags(ActivityFlags.NewTask);
        context.StartActivity(intent);
    }
}