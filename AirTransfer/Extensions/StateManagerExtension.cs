using AirTransfer.Interfaces;
using AirTransfer.Models;
using FantasyRemoteCopy.UI.Consts;

namespace AirTransfer.Extensions;

public static class StateManagerExtension
{
    private static void CreateDeviceContainer(IStateManager manager)
    {
        if (!manager.ExistKey(ConstParams.StateManagerKeys.DevicesKey))
            manager.SetState(ConstParams.StateManagerKeys.DevicesKey, new List<DiscoveredDeviceModel>());
    }

    public static void AddDiscoveryModel(this IStateManager manager, DiscoveredDeviceModel device)
    {
        CreateDeviceContainer(manager);
        manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey).Add(device);
    }

    public static void RemoveDiscoveryModel(this IStateManager manager, DiscoveredDeviceModel device)
    {
        CreateDeviceContainer(manager);
        var devices = manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey);
        if (devices.Exists(x => x.Flag == device.Flag)) devices.Remove(device);
    }

    public static bool ExistDiscoveryModel(this IStateManager manager, string? flag)
    {
        CreateDeviceContainer(manager);
        var devices = manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey);
        return devices.Exists(x => x.Flag == flag);
    }

    public static DiscoveredDeviceModel? FindDiscoveredDeviceModel(this IStateManager manager, string? flag)
    {
        CreateDeviceContainer(manager);
        return manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey)
            .FirstOrDefault(x => x.Flag == flag);
    }

    public static void ClearDiscoveryModel(this IStateManager manager)
    {
        CreateDeviceContainer(manager);
        manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey).Clear();
    }

    public static IReadOnlyList<DiscoveredDeviceModel> Devices(this IStateManager manager)
    {
        CreateDeviceContainer(manager);
        return manager.GetState<List<DiscoveredDeviceModel>>(ConstParams.StateManagerKeys.DevicesKey);
    }
}