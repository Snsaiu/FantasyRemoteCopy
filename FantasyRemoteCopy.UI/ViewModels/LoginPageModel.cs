using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class LoginPageModel : FantasyPageModelBase
    {
        private readonly INavigationService _navigationService;

        public LoginPageModel(IUserService userService, INavigationService navigationService)
        {
            this.userService = userService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string userName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string deviceNickName;

        private readonly IUserService userService;

        public bool CanLogin => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(DeviceNickName);

        [RelayCommand(CanExecute = nameof(CanLogin))]
        public async Task Login()
        {
            IsBusy = true;
            await userService.SaveUserAsync(new UserInfo() { Name = UserName, DeviceNickName = DeviceNickName });

            await _navigationService.NavigationToAsync(nameof(HomePage), false, null);
            //var homepage = App.Current.Services.GetService<HomePage>();
            //await Application.Current.MainPage.Navigation.PushAsync(homepage);
            //var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            //App.Current.MainPage.Navigation.RemovePage(removePage);
        }

        [RelayCommand]
        public async Task Init()
        {


            IsBusy = true;
            FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
            if (userRes.Ok)
            {
                UserName = userRes.Data.Name;
                DeviceNickName = userRes.Data.DeviceNickName;
                await _navigationService.NavigationToAsync(nameof(HomePage), false, null);
            }
            IsBusy = false;


        }
    }
}
