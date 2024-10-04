using Android.Content;
using Android.Webkit;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public class OpenFileProvider:IOpenFileable
{
    public void OpenFile(string filename)
    {
        var context = Android.App.Application.Context;
        Java.IO.File file = new Java.IO.File(filename);
                
        // 使用 FileProvider 获取 Uri，确保 Android 7.0 及以上版本兼容性
         var fileUri = FileProvider.GetUriForFile(context, context.PackageName + ".fileprovider", file);

        // 设置 Intent，指定动作和文件类型
        Intent intent = new Intent(Intent.ActionView);
        intent.SetDataAndType(fileUri, GetMimeType(filename));
        intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
        // 启动 Activity 打开文件
        context.StartActivity(intent);
    }
    
    private string GetMimeType(string filePath)
    {
        string extension = MimeTypeMap.GetFileExtensionFromUrl(filePath).ToLower();
        return MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension) ?? "*/*";
    }
}