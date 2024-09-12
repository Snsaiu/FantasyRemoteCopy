using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class SettingPageModel : FantasyPageModelBase
    {
        private readonly IUserService userService;
        private readonly IGlobalScanLocalNetIp _globalScanLocalNetIp;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
        [NotifyCanExecuteChangedFor(nameof(GlobalSearchCommand))]
        private bool isBusy = false;

        public bool IsNotBusy => !IsBusy;

        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public SettingPageModel(IUserService userService, IGlobalScanLocalNetIp globalScanLocalNetIp, INavigationService navigationService, IDialogService dialogService)
        {
            this.userService = userService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _globalScanLocalNetIp = globalScanLocalNetIp;
        }


        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private void GlobalSearch()
        {
            IsBusy = true;
            Task.Run(async () =>
            {
                await _globalScanLocalNetIp.GlobalSearch();

            }).GetAwaiter().OnCompleted(() =>
            {
                IsBusy = false;
                Application.Current.MainPage.DisplayAlert("Information", "Search Complete !", "Ok");
            });
        }

        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private async Task Logout()
        {
            await userService.ClearUserAsync();

            await _navigationService.NavigationToAsync(nameof(LoginPage), false, null);
            //var loginPage = App.Current.Services.GetService<LoginPage>();
            //await Application.Current.MainPage.Navigation.PushAsync(loginPage);
            //var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            //App.Current.MainPage.Navigation.RemovePage(removePage);
        }
    }
}
