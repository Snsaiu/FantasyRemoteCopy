using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.UI.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.ViewModels
{
    [ObservableObject]
    public partial class SettingPageModel
    {
        private readonly IUserService userService;
        private readonly IGlobalScanLocalNetIp _globalScanLocalNetIp;

        [ObservableProperty]
        [AlsoNotifyCanExecuteFor(nameof(LogoutCommand))]
        [AlsoNotifyCanExecuteFor(nameof(GlobalSearchCommand))]
        [AlsoNotifyChangeFor(nameof(IsNotBusy))]
        private bool isBusy = false;

        public bool IsNotBusy
        {
            get => !this.IsBusy;
        }

        public SettingPageModel(IUserService userService,IGlobalScanLocalNetIp globalScanLocalNetIp)
        {
            this.userService = userService;
            _globalScanLocalNetIp = globalScanLocalNetIp;
        }


        [ICommand(CanExecute = nameof(IsNotBusy))]
        public  void GlobalSearch()
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

        [ICommand(CanExecute =nameof(IsNotBusy))]
        public async void Logout()
        {
            await this.userService.ClearUser();

            var loginPage = App.Current.Services.GetService<LoginPage>();
            await Application.Current.MainPage.Navigation.PushAsync(loginPage);
            var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            App.Current.MainPage.Navigation.RemovePage(removePage);
        }
    }
}
