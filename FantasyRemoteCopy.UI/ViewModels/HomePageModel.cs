using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.UI.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.ViewModels
{
    [ObservableObject]
    public partial class HomePageModel
    {

        public HomePageModel(IUserService userService, SendDataBussiness sendDataBussiness) {
            this.userService = userService;
            this.sendDataBussiness = sendDataBussiness;
        }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string deviceNickName;
        private readonly IUserService userService;
        private readonly SendDataBussiness sendDataBussiness;

        [ICommand]
        public async void Init()
        {
            this.IsBusy = true;
            var userRes = await this.userService.GetCurrentUser();
            this.UserName = userRes.Data.Name;
            this.DeviceNickName = userRes.Data.DeviceNickName;
            this.IsBusy = false;
            this.deviceDiscover();
        }

        [ICommand]
        public  void Search()
        {
            this.deviceDiscover();
        }

        [ICommand]
        public async void GotoSettingPage()
        {
            var settingpage = App.Current.Services.GetService<SettingPage>();
            await Application.Current.MainPage.Navigation.PushAsync(settingpage);
        }

        /// <summary>
        /// 设备发现
        /// </summary>
        private  void deviceDiscover()
        {
            this.IsBusy= true;
            Task.Run(async () =>
            {
                await this.sendDataBussiness.DeviceDiscover();
            }).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(() =>
            {
                this.IsBusy = false;
            });
        }

    }
}
