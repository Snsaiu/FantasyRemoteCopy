using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.ViewModels.DialogModels;

public partial class SendTypeDialogModel : FantasyDialogModelBase
{
    public override event OnCloseDelegate OnCloseEvent;
    private DiscoveredDeviceModel discoveredDeviceModel;

    private readonly INavigationService _navigationService;


    private readonly DeviceLocalIpBase _deviceLocalIp;

    [ObservableProperty]
    private bool isBusy = false;

    public SendTypeDialogModel(DeviceLocalIpBase deviceLocalIp)
    {

        _deviceLocalIp = deviceLocalIp;
    }

    public override void OnParameter(INavigationParameter parameter)
    {
        discoveredDeviceModel = parameter.Get<DiscoveredDeviceModel>("data");
    }

    [RelayCommand]
    private Task TextInput()
    {
        OnCloseEvent(new CloseResultModel { Success = true, Data = SendType.Text });
        return Task.CompletedTask;
    }

    [RelayCommand]

    public async Task FileInput()
    {

        FileResult? f = await FilePicker.PickAsync();

        if (f != null)
        {
            string ip = await _deviceLocalIp.GetLocalIpAsync();

            SendFileModel sendfileModel = new SendFileModel(ip, discoveredDeviceModel.Flag, f.FullPath);
            OnCloseEvent(new CloseResultModel { Success = true, Data = sendfileModel });
        }
        else
        {
            OnCloseEvent(new CloseResultModel { Success = false });
        }

    }
}