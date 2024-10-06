using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class LoginPageModel(IUserService userService, INavigationService navigationService)
    : ViewModelBase
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string deviceNickName = string.Empty;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string userName = string.Empty;

    public bool CanLogin => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(DeviceNickName);

    [RelayCommand(CanExecute = nameof(CanLogin))]
    public async Task Login()
    {
        IsBusy = true;
        await userService.SaveUserAsync(new UserInfo { Name = UserName, DeviceNickName = DeviceNickName });
        await navigationService.NavigationToAsync(nameof(HomePage), false);
    }

    [RelayCommand]
    public async Task Init()
    {
        IsBusy = true;
        await Task.Yield();
        FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
        if (userRes.Ok)
        {
            UserName = userRes.Data.Name;
            DeviceNickName = userRes.Data.DeviceNickName;
            await navigationService.NavigationToAsync(nameof(HomePage), false);
        }

        IsBusy = false;
        await Task.Yield();
    }
}