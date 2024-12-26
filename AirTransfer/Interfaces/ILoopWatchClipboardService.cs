namespace AirTransfer.Interfaces;

public interface ILoopWatchClipboardService
{
    void SetState(bool state);

    bool GetState();
}