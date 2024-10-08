using System.IO.Compression;

namespace FantasyRemoteCopy.UI.Tools;

public static class ZipHelper
{
    public static void CreateZipFromFolder(string folderPath, string zipFilePath)
    {
        // 删除现有的ZIP文件
        if (File.Exists(zipFilePath)) File.Delete(zipFilePath);
        // 创建新的ZIP文件
        ZipFile.CreateFromDirectory(folderPath, zipFilePath, CompressionLevel.Fastest, true);
    }
}