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

        public SettingPageModel(IUserService userService) {
            this.userService = userService;
        }


        [ICommand]
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
