using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.UI.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.ViewModels
{
  
    public partial class SettingPageModel:FantasyPageModelBase
    {
        private readonly IUserService userService;
        private readonly IGlobalScanLocalNetIp _globalScanLocalNetIp;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
        [NotifyCanExecuteChangedFor(nameof(GlobalSearchCommand))]
        private bool isBusy = false;

        public bool IsNotBusy
        {
            get => !this.IsBusy;
        }

        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public SettingPageModel(IUserService userService,IGlobalScanLocalNetIp globalScanLocalNetIp,INavigationService navigationService,IDialogService dialogService)
        {
            this.userService = userService;
            this._dialogService = dialogService;
            this._navigationService = navigationService;
            _globalScanLocalNetIp = globalScanLocalNetIp;
        }


        [RelayCommand(CanExecute = nameof(IsNotBusy))]
        private  void GlobalSearch()
        {
            this.IsBusy = true;
            Task.Run( async() =>
            { 
               await this._globalScanLocalNetIp.GlobalSearch();

            }).GetAwaiter().OnCompleted(() =>
            {
                this.IsBusy = false; 
                Application.Current.MainPage.DisplayAlert("Information", "Search Complete !", "Ok");
            });
        }

        [RelayCommand(CanExecute =nameof(IsNotBusy))]
        private async Task Logout()
        {
            await this.userService.ClearUser();

             await this._navigationService.NavigationToAsync(nameof(LoginPage), false, null);
            //var loginPage = App.Current.Services.GetService<LoginPage>();
            //await Application.Current.MainPage.Navigation.PushAsync(loginPage);
            //var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            //App.Current.MainPage.Navigation.RemovePage(removePage);
        }
    }
}
