using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces;

public interface IGetDevices
{
    IAsyncEnumerable<object> GetDevicesAsync(CancellationToken cancellationToken);
}

public interface IGetDevice<out T> : IGetDevices where T : ScanDevice
{
    new IAsyncEnumerable<T> GetDevicesAsync(CancellationToken cancellationToken);

    IAsyncEnumerable<object> IGetDevices.GetDevicesAsync(CancellationToken cancellationToken)
    {
        return GetDevicesAsync(cancellationToken);
    }

}

