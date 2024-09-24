using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class TextInputPageModel : FantasyPageModelBase, INavigationAware
{
    private INavigationService _navigationService;
    private readonly DeviceLocalIpBase _localIpBase;

    [ObservableProperty] private string content;

    public TextInputPageModel(INavigationService navigationService, DeviceLocalIpBase localIpBase)
    {
        _navigationService = navigationService;
        _localIpBase = localIpBase;
    }

    private Models.DiscoveredDeviceModel _discoveredDeviceModel;


    [RelayCommand]
    private async Task SendData()
    {
        if (string.IsNullOrEmpty(Content))
            await _navigationService.NavigationToAsync(nameof(Views.HomePage), false, null);

        var ip = await this._localIpBase.GetLocalIpAsync();

        SendTextModel model = new SendTextModel(ip, _discoveredDeviceModel.Flag,Content);
        NavigationParameter para = new NavigationParameter();
        para.Add("data", model);
        await _navigationService.NavigationToAsync(nameof(Views.HomePage), false, para);
    }

    [ObservableProperty] private string title = "To: ";

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {
    }

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
        _discoveredDeviceModel = parameter.Get<Models.DiscoveredDeviceModel>("data");
        Title += _discoveredDeviceModel.NickName;
    }
}