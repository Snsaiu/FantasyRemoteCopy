using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

using DataType = FantasyRemoteCopy.UI.Models.DataType;

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

            FileInfo finfo = new FileInfo(f.FullPath);
            if (finfo.Length > 1073741824)
            {

                await App.Current.MainPage.DisplayAlert("Warning", "File cannot be larger than 1G ！", "Ok");
                return;
            }
            IsBusy = true;
//             if (res != null)
//             {
//                 IsBusy = false;
// #if WINDOWS
//                   await Application.Current.MainPage.DisplayAlert("Information", "Sended!", "Ok");
// #endif
//
//
//                 OnCloseEvent(new CloseResultModel { Success = true });
//
//             }



        }
        OnCloseEvent(new CloseResultModel { Success = false });
    }
}