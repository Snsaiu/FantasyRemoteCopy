using AirTransfer.Interfaces;
using AirTransfer.Native;

namespace AirTransfer;

public sealed class TrayService : ITrayService
{
    private WindowsTrayIcon tray;

    public Action ClickHandler { get; set; }

    public void Initialize()
    {
        tray = new("Platforms\\Windows\\trayicon.ico");
        tray.LeftClick = () =>
        {
            WindowExtensions.BringToFront();
            ClickHandler?.Invoke();
        };
    }
}