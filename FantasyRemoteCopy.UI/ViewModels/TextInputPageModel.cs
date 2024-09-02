
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;

namespace FantasyRemoteCopy.UI.ViewModels;


public partial class TextInputPageModel : FantasyPageModelBase, INavigationAware
{
    private SendDataBussiness _sendDataBussiness;

    private INavigationService _navigationService;

    [ObservableProperty]
    private string content;

    public TextInputPageModel(SendDataBussiness sendDataBussiness,INavigationService navigationService)
    {
        this._navigationService = navigationService;
        this._sendDataBussiness = sendDataBussiness;

    }

    private Models.DiscoveredDeviceModel _discoveredDeviceModel;


    [RelayCommand]
    private async Task SendData()
    {
        await this._sendDataBussiness.SendData(this._discoveredDeviceModel.Ip, this.Content, DataType.Text);
        await Task.Delay(1000);
       await this._navigationService.NavigationToAsync(nameof(Views.HomePage), false, null);
    }

    [ObservableProperty]
    private string title = "To: ";

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {
       
    }

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
        this._discoveredDeviceModel = parameter.Get<Models.DiscoveredDeviceModel>("data");
        this.Title += this._discoveredDeviceModel.NickName;
    }
}