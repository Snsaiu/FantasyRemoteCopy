using AirTransfer.Interfaces.Impls.Configs;
using AirTransfer.Interfaces.Impls.TcpTransfer;

namespace AirTransfer.Interfaces;

public class TcpLoopListenFactory
{
    public static TcpLoopListenContentBase Create()
    {
        var fileSavePath = Application.Current.MainPage.Handler.MauiContext.Services
            .GetRequiredService<FileSavePathBase>();
        return new TcpLoopListenContent(fileSavePath);
    }
}