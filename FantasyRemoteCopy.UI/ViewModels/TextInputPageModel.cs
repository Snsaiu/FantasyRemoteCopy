
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyNavigation;

using DataType = FantasyRemoteCopy.UI.Models.DataType;
using SendDataBussiness = FantasyRemoteCopy.UI.Bussiness.SendDataBussiness;

namespace FantasyRemoteCopy.UI.ViewModels;


public partial class TextInputPageModel : FantasyPageModelBase, INavigationAware
{
    private readonly SendDataBussiness _sendDataBussiness;

    private INavigationService _navigationService;

    [ObservableProperty]
    private string content;

    public TextInputPageModel(SendDataBussiness sendDataBussiness, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _sendDataBussiness = sendDataBussiness;

    }

    private Models.DiscoveredDeviceModel _discoveredDeviceModel;


    [RelayCommand]
    private async Task SendData()
    {
        await _sendDataBussiness.SendData(_discoveredDeviceModel.Ip, Content, DataType.Text);
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