using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;

namespace FantasyRemoteCopy.UI.Interfaces;

public class TcpLoopListenFactory
{
    public static TcpLoopListenContentBase Create()
    {
        FileSavePathBase fileSavePath = FantasyMvvm.FantasyContainer.GetRequiredService<FileSavePathBase>();
        return new TcpLoopListenContent(fileSavePath);
    }
}