using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces;

public interface IGetDevices
{
    IAsyncEnumerable<object> GetDevicesAsync(CancellationToken cancellationToken);
}

public interface IGetDevices<out T> : IGetDevices where T : ScanDevice
{
    new IAsyncEnumerable<T> GetDevicesAsync(CancellationToken cancellationToken);

    IAsyncEnumerable<object> IGetDevices.GetDevicesAsync(CancellationToken cancellationToken)
    {
        return GetDevicesAsync(cancellationToken);
    }

}

/// <summary>
/// 获得本地局域网的设备
/// </summary>
public interface IGetLocalNetDevices : IGetDevices<ScanDevice>
{
    
}

/// <summary>
/// 获得本地蓝牙设备
/// </summary>
public interface IGetBluetoothDevices : IGetDevices<ScanDevice>
{
    
}

