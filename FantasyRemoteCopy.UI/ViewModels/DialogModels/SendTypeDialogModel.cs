using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels.DialogModels;

public partial class SendTypeDialogModel : FantasyDialogModelBase
{
    public override event OnCloseDelegate OnCloseEvent;
    private DiscoveredDeviceModel discoveredDeviceModel;

    private readonly INavigationService _navigationService;

    private readonly IDialogService _dialogService;
    private readonly DeviceLocalIpBase _deviceLocalIp;

    [ObservableProperty]
    private bool isBusy = false;

    public SendTypeDialogModel(INavigationService navigationService, IDialogService dialogService,DeviceLocalIpBase deviceLocalIp)
    {
        _dialogService = dialogService;
        _deviceLocalIp = deviceLocalIp;
        _navigationService = navigationService;
    }

    public override void OnParameter(INavigationParameter parameter)
    {
        discoveredDeviceModel = parameter.Get<DiscoveredDeviceModel>("data");
    }

    [RelayCommand]
    private async Task TextInput()
    {
        NavigationParameter parameter = new NavigationParameter();
        parameter.Add("data", discoveredDeviceModel);

        await _navigationService.NavigationToAsync(nameof(TextInputPage), parameter);

        OnCloseEvent(new CloseResultModel { Success = false });
    }

    [RelayCommand]

    public async Task FileInput()
    {

        FileResult? f = await FilePicker.PickAsync();

        if (f != null)
        {
            var ip = await this._deviceLocalIp.GetLocalIpAsync();
            
            SendFileModel sendfileModel = new SendFileModel(ip,discoveredDeviceModel.Flag, f.FullPath);
            OnCloseEvent(new CloseResultModel { Success = true, Data = sendfileModel });
        }
        else
        {
            OnCloseEvent(new CloseResultModel { Success = false });
        }

    }
}