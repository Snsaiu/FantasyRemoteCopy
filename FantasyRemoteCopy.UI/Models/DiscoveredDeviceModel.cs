using CommunityToolkit.Mvvm.ComponentModel;

namespace FantasyRemoteCopy.UI.Models;

public enum WorkState
{
    None,
    Sending,
    Downloading
}

/// <summary>
///     设备发现的模型
/// </summary>
public partial class DiscoveredDeviceModel : DeviceModel
{
    [ObservableProperty] private bool isChecked;

    [ObservableProperty] private double progress;

    [ObservableProperty] private WorkState workState;

    public List<TransmissionTaskModel> TransmissionTasks { get; } = [];

    public bool TryGetTransmissionTask(string taskId, out TransmissionTaskModel? task)
    {
        task = null;
        if (TransmissionTasks.Any(x => x.TaskGuid == taskId))
            task = TransmissionTasks.FirstOrDefault(x => x.TaskGuid == taskId);
        return task != null;
    }

    public void RemoveTransmissionTask(string taskId)
    {
        TransmissionTasks.Remove(TransmissionTasks.FirstOrDefault(x => x.TaskGuid == taskId));
    }

    public DiscoveredDeviceModel()
    {
    }

    public DiscoveredDeviceModel(DeviceModel device)
    {
        NickName = device.NickName;
        DeviceType = device.DeviceType;
        SystemType = device.SystemType;
        Flag = device.Flag;
        DeviceName = device.DeviceName;
    }

    public static implicit operator DiscoveredDeviceModel(JoinMessageModel model)
    {
        return new DiscoveredDeviceModel
        {
            NickName = model.DeviceName,
            Flag = model.Flag,
            SystemType = model.SystemType,
            DeviceType = model.Device.ToString()
        };
    }
}