using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels.DialogModels;

public partial class SendTypeDialogModel : FantasyDialogModelBase
{
    public override event OnCloseDelegate OnCloseEvent;
    private DiscoveredDeviceModel discoveredDeviceModel;

    private readonly INavigationService _navigationService;

    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private bool isBusy = false;

    public SendTypeDialogModel(INavigationService navigationService, IDialogService dialogService)
    {
        _dialogService = dialogService;
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
            SendFileModel sendfileModel = new SendFileModel(discoveredDeviceModel.Flag, f.FullPath);
            OnCloseEvent(new CloseResultModel { Success = true, Data = sendfileModel });
        }
        else
        {
            OnCloseEvent(new CloseResultModel { Success = false });
        }

    }
}