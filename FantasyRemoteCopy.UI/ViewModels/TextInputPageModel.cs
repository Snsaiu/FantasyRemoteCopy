
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Models;

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
    private Task SendData()
    {
        if (string.IsNullOrEmpty(Content))
            return _navigationService.NavigationToAsync(nameof(Views.HomePage), false, null);

        SendTextModel model = new SendTextModel(_discoveredDeviceModel.Flag, Content);
        NavigationParameter para = new NavigationParameter();
        para.Add("data", model);
        return _navigationService.NavigationToAsync(nameof(Views.HomePage), false, para);
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