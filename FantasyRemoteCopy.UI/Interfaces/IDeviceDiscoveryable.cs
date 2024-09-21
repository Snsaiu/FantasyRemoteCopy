namespace FantasyRemoteCopy.UI.Interfaces;

/// <summary>
/// 设备发现
/// </summary>
public interface IDeviceDiscoveryable
{
    bool Stop { get; set; }
    Task DiscoverDevicesAsync(Action<object> discoveryCallBack);
}

public interface IDeviceDiscoveryable<T>:IDeviceDiscoveryable
{
    Task DiscoverDevicesAsync(Action<T> discoveryCallBack);

    Task IDeviceDiscoveryable.DiscoverDevicesAsync(Action<object> discoveryCallBack)
    {
        if(discoveryCallBack is Action<T> action)
            discoveryCallBack(action);
        else
            throw new NotImplementedException();
        return Task.CompletedTask;
    }
}