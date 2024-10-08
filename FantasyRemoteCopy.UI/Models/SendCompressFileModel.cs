using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Tools;

namespace FantasyRemoteCopy.UI.Models;

public class SendCompressFileModel : SendFileModel, ICompress
{
    public SendCompressFileModel(SendFolderModel folderModel) : this(folderModel.Flag, folderModel.TargetFlag,
        folderModel.FileFullPath)
    {
    }

    public SendCompressFileModel(string flag, string targetFlag, string fileFullPath) : base(flag, targetFlag,
        fileFullPath)
    {
        if (!Directory.Exists(FileFullPath))
            throw new DirectoryNotFoundException();
        var zipName =Path.GetFileName(fileFullPath)+ ".zip";
        var compressPath = Path.Combine( FileSystem.CacheDirectory,zipName );

            ZipHelper.CreateZipFromFolder(fileFullPath, compressPath);
            FileFullPath= compressPath;
  
       
        FileFullPath = compressPath;
    }
}