using CommunityToolkit.Mvvm.ComponentModel;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models;

public partial class DeviceModel : ObservableObject, IFlag
{
    [ObservableProperty] private string? deviceName;

    [ObservableProperty] private string? deviceType;

    [ObservableProperty] private string? flag;

    [ObservableProperty] private string? nickName;

    [ObservableProperty] private SystemType systemType;
}