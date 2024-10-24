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

    [ObservableProperty]
    private string content = string.Empty;

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {
    }

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
     
    }

    private bool CanSend()
    {
        return !string.IsNullOrWhiteSpace(Content);
    }

    partial void OnContentChanged(string value)
    {
       SendDataCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute =(nameof(CanSend)))]
    private async Task SendData()
    {
        if (string.IsNullOrEmpty(Content))
            await navigationService.NavigationToAsync(nameof(HomePage), false);

        InformationModel model = new()
        {
            SendType=Enums.SendType.Text,
            Text=Content
        };

        NavigationParameter para = new();
        para.Add("data", model);
        await navigationService.NavigationToAsync(nameof(HomePage), false, para);
    }
}