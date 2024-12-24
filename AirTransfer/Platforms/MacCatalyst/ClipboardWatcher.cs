using AirTransfer.Interfaces;

namespace AirTransfer;

public sealed class ClipboardWatcher : IClipboardWatchable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Action<object>? ClipboardUpdate { get; set; }

    public void Initialize(object parameter)
    {
        throw new NotImplementedException();
    }
}