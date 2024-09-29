using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class TextInputPageModel(INavigationService navigationService, DeviceLocalIpBase localIpBase)
    : ViewModelBase, INavigationAware
{
    private DiscoveredDeviceModel? _discoveredDeviceModel;
    [ObservableProperty] private string content = string.Empty;

    [ObservableProperty] private string title = "To: ";

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {
    }

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
        _discoveredDeviceModel = parameter.Get<DiscoveredDeviceModel>("data");
        Title += _discoveredDeviceModel.NickName;
    }


    [RelayCommand]
    private async Task SendData()
    {
        if (string.IsNullOrEmpty(Content))
            await navigationService.NavigationToAsync(nameof(HomePage), false);

        string ip = await localIpBase.GetLocalIpAsync();

        SendTextModel model = new SendTextModel(ip, _discoveredDeviceModel?.Flag ?? throw new NullReferenceException(), Content);
        NavigationParameter para = new NavigationParameter();
        para.Add("data", model);
        await navigationService.NavigationToAsync(nameof(HomePage), false, para);
    }
}