using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;

namespace FantasyRemoteCopy.UI.ViewModels.DialogModels;

public partial class SendTypeDialogModel(DeviceLocalIpBase deviceLocalIp) : DialogModelBase
{
    public override event OnCloseDelegate? OnCloseEvent;
    private DiscoveredDeviceModel? discoveredDeviceModel;

    [ObservableProperty] private bool isBusy;

    public override void OnParameter(INavigationParameter parameter)
    {
        discoveredDeviceModel = parameter.Get<DiscoveredDeviceModel>("data");
    }

    [RelayCommand]
    private Task TextInput()
    {
        OnCloseEvent?.Invoke(new CloseResultModel { Success = true, Data = SendType.Text });
        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task FileInputAsync()
    {
        FileResult? f = await FilePicker.PickAsync();

        if (f != null)
        {
            string ip = await deviceLocalIp.GetLocalIpAsync();
            if (discoveredDeviceModel is null)
                throw new NullReferenceException();
            SendFileModel sendfileModel = new SendFileModel(ip,
                discoveredDeviceModel.Flag ?? throw new NullReferenceException(), f.FullPath);
            OnCloseEvent?.Invoke(new CloseResultModel { Success = true, Data = sendfileModel });
        }
        else
        {
            OnCloseEvent?.Invoke(new CloseResultModel { Success = false });
        }
    }

    [RelayCommand]
    public async Task FolderInputAsync()
    {
        var f = await FolderPicker.PickAsync(default);
        if (!f.IsSuccessful)
        {
            OnCloseEvent?.Invoke(new CloseResultModel { Success = false });
            return;
        }
        
        var ip = await deviceLocalIp.GetLocalIpAsync();
        if (discoveredDeviceModel is null)
            throw new NullReferenceException();
        var sendFolderModel = new SendFolderModel(ip, discoveredDeviceModel.Flag ?? throw new NullReferenceException(),
            f.Folder?.Path ?? throw new NullReferenceException());
        OnCloseEvent?.Invoke(new CloseResultModel { Success = true, Data = sendFolderModel });
    }
}