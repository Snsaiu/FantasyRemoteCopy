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
using FantasyRemoteCopy.UI.Views.Dialogs;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyRegionManager;
using FantasyMvvm.FantasyNavigation;
using FantasyMvvm.FantasyModels.Impls;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class HomePageModel:FantasyPageModelBase
    {
        private readonly IDialogService _dialogService = null;

        private readonly IRegionManager _regionManager = null;

        private readonly INavigationService _navigationService = null;

        public HomePageModel(IUserService userService, SendDataBussiness sendDataBussiness,ReceiveBussiness receiveBussiness,ISaveDataService dataService,IFileSaveLocation fileSaveLocation,IDialogService dialogService,IRegionManager regionManager,INavigationService navigationService) {
            this.userService = userService;
            this.sendDataBussiness = sendDataBussiness;
            this.receiveBussiness = receiveBussiness;
            _dataService = dataService;
            _fileSaveLocation = fileSaveLocation;
            this._dialogService = dialogService;

            this._regionManager = regionManager;

            this._navigationService = navigationService;
           
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

                if (d.Type == Core.Enums.TransformType.SendingTxtData)
                {
                    SaveDataModel sdm=new SaveDataModel();
                    sdm.Content= str;
                    sdm.DataType = SaveDataType.Txt;
                    sdm.Guid = d.DataGuid;
                    sdm.Time = DateTime.Now;
                    sdm.SourceDeviceNickName = d.TargetDeviceNickName;
                    this._dataService.AddAsync(sdm);
                    this.NewMessageVisible=true;
                    this.IsDownLoadingVisible = false;
                }
                else
                {
                   var sdm=  JsonConvert.DeserializeObject<FileDataModel>(str);
                   var saveLocation = this._fileSaveLocation.GetSaveLocation();
                   var fileFullName = Path.Combine(saveLocation, sdm.FileNameWithExtension);
                   if (File.Exists(fileFullName))
                   {
                       string onlyFileName = Path.GetFileNameWithoutExtension(fileFullName);
                       string extension = Path.GetExtension(fileFullName);
                       var newFileName = onlyFileName + "_" + Guid.NewGuid().ToString().Substring(1, 4) + extension;
                       fileFullName=Path.Combine(saveLocation, newFileName);
                   }

                    Task.Run(() =>
                   {
                     
                       using (FileStream fs = new FileStream(fileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                       {
                           fs.Write(sdm.ContentBytes, 0, sdm.ContentBytes.Length);
                       }

                   }).GetAwaiter().OnCompleted(() =>
                   {

                       SaveDataModel sdm = new SaveDataModel();
                       sdm.Content = fileFullName;
                       sdm.DataType = SaveDataType.File;
                       sdm.Guid = d.DataGuid;
                       sdm.Time = DateTime.Now;
                       sdm.SourceDeviceNickName = d.TargetDeviceNickName;
                       this._dataService.AddAsync(sdm);

                       this.NewMessageVisible = true;
                       this.IsDownLoadingVisible=false;

                   });

                }



            };

            this.receiveBussiness.ReceivingDataEvent += (ip) =>
            {
                this.IsDownLoadingVisible = true;
                if (DiscoveredDevices != null)
                {
                    var find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.IsDownLoading = true;
                    }
                }
            };

            this.receiveBussiness.ReceivedFileFinishedEvent += (ip) =>
            {
                if (DiscoveredDevices != null)
                {
                    var find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.IsDownLoading = false;
                        find.DownloadProcess = 0;
                    }
                }
            };

            this.receiveBussiness.ReceivingProcessEvent += (ip,process) =>
            {
                if (DiscoveredDevices != null)
                {
                    var find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.DownloadProcess = process;
                    }
                }
            };

            this.sendDataBussiness.SendingDataEvent += (ip) =>
            {
                if (DiscoveredDevices != null)
                {
                    var find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.IsSendingData = true;
                    }
                }
            };
            this.sendDataBussiness.SendFinishedEvent += (ip) =>
            {

                if (DiscoveredDevices != null)
                {
                    var find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.IsSendingData = false;
                    }
                }
            };

        }

        [ObservableProperty]
        private bool isDownLoadingVisible;

        [ObservableProperty]
        private bool newMessageVisible = false;

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
        private readonly ISaveDataService _dataService;
        private readonly IFileSaveLocation _fileSaveLocation;

        [RelayCommand]
        public async void Init()
        {
            this.IsBusy = true;
            var userRes = await this.userService.GetCurrentUser();
            this.UserName = userRes.Data.Name;
            this.DeviceNickName = userRes.Data.DeviceNickName;
            this.IsBusy = false;
            this.deviceDiscover();
        }

        [RelayCommand]
        public  void Search()
        {
            this.deviceDiscover();
        }


        [RelayCommand]
        public async void GotoList()
        {
            this.NewMessageVisible = false;

            await this._navigationService.NavigationToAsync(nameof(ListPage), null);


        }


        [RelayCommand]
        public async void Share(DiscoveredDeviceModel model)
        {
            if(model.IsSendingData)
            {
               
             await this._dialogService.DisplayAlert("Warning", "Sorry, the file is being uploaded. Please try again after the upload is completed!","Ok");
                return;
            }


            NavigationParameter parameter = new NavigationParameter();
            parameter.Add("data", model);
           await this._dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, null);
        }

        [RelayCommand]
        public async void GotoSettingPage()
        {
        

            await this._navigationService.NavigationToAsync(nameof(SettingPage), null);

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
            }).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(async () =>
            {
                await Task.Delay(1000);
                this.IsBusy = false;

                if(this.DiscoveredDevices.Count==0)
                {
                   
                    await Application.Current.MainPage.DisplayAlert("warning", "No connectable devices found! ", "Ok");
                }
            });
        }

    }
}
