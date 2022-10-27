using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.ViewModels
{
    [ObservableObject]
    public partial class HomePageModel
    {

        public HomePageModel(IUserService userService, SendDataBussiness sendDataBussiness,ReceiveBussiness receiveBussiness) {
            this.userService = userService;
            this.sendDataBussiness = sendDataBussiness;
            this.receiveBussiness = receiveBussiness;

            this.DiscoveredDevices = new ObservableCollection<DiscoveredDeviceModel>();
            
            //发现可用的设备回调
            this.receiveBussiness.DiscoverEnableIpEvent += (model) =>
            {
                DiscoveredDeviceModel ddm = new DiscoveredDeviceModel();
                ddm.DeviceType = model.DevicePlatform;
                ddm.DeviceName=model.DeviceName;
                ddm.NickName = model.NickName;
                ddm.Ip = model.DeviceIP;

                if (ddm.DeviceType == "WinUI")
                    ddm.Img = ImageSource.FromFile("windows.png");
                if (ddm.DeviceType == "MacCatalyst")
                    ddm.Img = ImageSource.FromFile("mac.png");

                this.DiscoveredDevices.Add(ddm);



            };

            this.receiveBussiness.ReceiveDataEvent += (d) =>
            {
                var str = Encoding.UTF8.GetString(d.Data);
                var dm = JsonConvert.DeserializeObject<DataMetaModel>(str);


            };


        }

        [ObservableProperty]
        private bool isBusy = false;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private ObservableCollection<DiscoveredDeviceModel> discoveredDevices;

        [ObservableProperty]
        private string deviceNickName;
        private readonly IUserService userService;
        private readonly SendDataBussiness sendDataBussiness;
        private readonly ReceiveBussiness receiveBussiness;

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
            this.DiscoveredDevices.Clear();
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
