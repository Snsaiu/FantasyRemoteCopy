using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels
{
    
    public partial class LoginPageModel:FantasyPageModelBase
    {
        private readonly INavigationService _navigationService;

        public LoginPageModel(IUserService userService,INavigationService navigationService)
        {
            this.userService = userService;
            this._navigationService = navigationService;
        }

        [ObservableProperty]
        private bool isBusy=false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string userName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string deviceNickName;

        private readonly IUserService userService;

        public bool CanLogin {
            get => !string.IsNullOrWhiteSpace(this.UserName)&&!string.IsNullOrWhiteSpace(this.DeviceNickName);
        }

        [RelayCommand(CanExecute =nameof(CanLogin))]
        public async Task Login()
        {
            this.IsBusy = true;
            await this.userService.SaveUser(new Core.Models.UserInfo() { Name = this.UserName, DeviceNickName = this.DeviceNickName });

           await this._navigationService.NavigationToAsync(nameof(HomePage), false, null);
            //var homepage = App.Current.Services.GetService<HomePage>();
            //await Application.Current.MainPage.Navigation.PushAsync(homepage);
            //var removePage = App.Current.MainPage.Navigation.NavigationStack.First();
            //App.Current.MainPage.Navigation.RemovePage(removePage);
        }

        [RelayCommand]
        public async Task Init()
        {
           

            this.IsBusy=true;
            var userRes = await this.userService.GetCurrentUser();
            if(userRes.Ok)
            {
                this.UserName = userRes.Data.Name;
                this.DeviceNickName = userRes.Data.DeviceNickName;
                await this._navigationService.NavigationToAsync(nameof(HomePage), false, null);
            }
            this.IsBusy = false;

           
        }
    }
}
