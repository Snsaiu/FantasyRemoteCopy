
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyNavigation;

using DataType = FantasyRemoteCopy.UI.Models.DataType;

namespace FantasyRemoteCopy.UI.ViewModels;


public partial class TextInputPageModel : FantasyPageModelBase, INavigationAware
{
    private INavigationService _navigationService;

    [ObservableProperty]
    private string content;

    public TextInputPageModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    private Models.DiscoveredDeviceModel _discoveredDeviceModel;


    [RelayCommand]
    private async Task SendData()
    {
        await Task.Delay(1000);
        await _navigationService.NavigationToAsync(nameof(Views.HomePage), false, null);
    }

    [ObservableProperty]
    private string title = "To: ";

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {

    }

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
        _discoveredDeviceModel = parameter.Get<Models.DiscoveredDeviceModel>("data");
        Title += _discoveredDeviceModel.NickName;
    }
}