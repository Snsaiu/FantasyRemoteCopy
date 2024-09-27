using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class SettingPageModel(
    IUserService userService,
    DeviceLocalIpBase getLocalIp,
    INavigationService navigationService,
    IDialogService dialogService,
    GlobalScanBase globalScan)
    : FantasyPageModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
    [NotifyCanExecuteChangedFor(nameof(GlobalSearchCommand))]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task GlobalSearchAsync()
    {
        try
        {
            IsBusy = true;
            var ip = await getLocalIp.GetLocalIpAsync();
            await globalScan.SendAsync(ip, default);
        }
        finally
        {
            IsBusy = false;
            await dialogService.DisplayAlert("Information", "Search Complete !", "Ok");
        }
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task Logout()
    {
        await userService.ClearUserAsync();
        await navigationService.NavigationToAsync(nameof(LoginPage), false);
    }
}