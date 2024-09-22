using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class SettingPageModel : FantasyPageModelBase
    {
        private readonly IUserService _userService;
        private readonly DeviceLocalIpBase _getLocalIp;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
        [NotifyCanExecuteChangedFor(nameof(GlobalSearchCommand))]
        private bool isBusy = false;

        public bool IsNotBusy => !IsBusy;

        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly GlobalScanBase _globalScan;

        public SettingPageModel(IUserService userService,DeviceLocalIpBase getLocalIp, INavigationService navigationService, IDialogService dialogService,GlobalScanBase globalScan)
        {
            this._userService = userService;
            _getLocalIp = getLocalIp;
            _dialogService = dialogService;
            _globalScan = globalScan;
            _navigationService = navigationService;
            
        }


        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private async Task GlobalSearchAsync()
        {
            try
            {
                IsBusy = true;
                var ip = await this._getLocalIp.GetLocalIpAsync();
                await this._globalScan.SendAsync(ip);
            }
            finally
            {
                IsBusy = false;
                
               await Application.Current.MainPage.DisplayAlert("Information", "Search Complete !", "Ok");
            }
      
            
            // IsBusy = true;
            // Task.Run(async () =>
            // {
            //     await _globalScanLocalNetIp.GlobalSearch();
            //
            // }).GetAwaiter().OnCompleted(() =>
            // {
            //     IsBusy = false;
            //     
            // });
        }

        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private async Task Logout()
        {
            await _userService.ClearUserAsync();

            await _navigationService.NavigationToAsync(nameof(LoginPage), false, null);
            //var loginPage = App.Current.Services.GetService<LoginPage>();
            //await Application.Current.MainPage.Navigation.PushAsync(loginPage);
            //var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            //App.Current.MainPage.Navigation.RemovePage(removePage);
        }
    }
}
