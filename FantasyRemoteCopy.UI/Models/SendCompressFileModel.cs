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
        var compressPath = Path.Combine(fileFullPath, Guid.NewGuid() + ".zip");
        ZipHelper.CreateZipFromFolder(fileFullPath, compressPath);
        FileFullPath = compressPath;
    }
}