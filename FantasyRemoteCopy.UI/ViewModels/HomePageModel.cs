using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyMvvm.FantasyRegionManager;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

using Newtonsoft.Json;

using System.Collections.ObjectModel;
using System.Text;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class HomePageModel : FantasyPageModelBase
    {
        private readonly IDialogService _dialogService = null;

        private readonly IRegionManager _regionManager = null;

        private readonly INavigationService _navigationService = null;

        public HomePageModel(IUserService userService, SendDataBussiness sendDataBussiness, ReceiveBussiness receiveBussiness, ISaveDataService dataService, IFileSaveLocation fileSaveLocation, IDialogService dialogService, IRegionManager regionManager, INavigationService navigationService)
        {
            this.userService = userService;
            this.sendDataBussiness = sendDataBussiness;
            this.receiveBussiness = receiveBussiness;
            _dataService = dataService;
            _fileSaveLocation = fileSaveLocation;
            _dialogService = dialogService;

            _regionManager = regionManager;

            _navigationService = navigationService;

            DiscoveredDevices = [];

            //发现可用的设备回调
            this.receiveBussiness.DiscoverEnableIpEvent += (model) =>
            {
                DiscoveredDeviceModel ddm = new DiscoveredDeviceModel
                {
                    DeviceType = model.DevicePlatform,
                    DeviceName = model.DeviceName,
                    NickName = model.NickName,
                    Ip = model.DeviceIP
                };

                if (ddm.DeviceType == "WinUI")
                    ddm.Img = ImageSource.FromFile("windows.png");
                if (ddm.DeviceType == "MacCatalyst")
                    ddm.Img = ImageSource.FromFile("mac.png");

                DiscoveredDevices.Add(ddm);



            };


            this.receiveBussiness.ReceiveDataEvent += (d) =>
            {
                string str = Encoding.UTF8.GetString(d.Data);

                if (d.Type == Core.Enums.TransformType.SendingTxtData)
                {
                    SaveDataModel sdm = new SaveDataModel
                    {
                        Content = str,
                        DataType = SaveDataType.Txt,
                        Guid = d.DataGuid,
                        Time = DateTime.Now,
                        SourceDeviceNickName = d.TargetDeviceNickName
                    };
                    _dataService.AddAsync(sdm);
                    NewMessageVisible = true;
                    IsDownLoadingVisible = false;
                }
                else
                {
                    FileDataModel sdm = JsonConvert.DeserializeObject<FileDataModel>(str);
                    string saveLocation = _fileSaveLocation.GetSaveLocation();
                    string fileFullName = Path.Combine(saveLocation, sdm.FileNameWithExtension);
                    if (File.Exists(fileFullName))
                    {
                        string onlyFileName = Path.GetFileNameWithoutExtension(fileFullName);
                        string extension = Path.GetExtension(fileFullName);
                        string newFileName = onlyFileName + "_" + Guid.NewGuid().ToString().Substring(1, 4) + extension;
                        fileFullName = Path.Combine(saveLocation, newFileName);
                    }

                    Task.Run(() =>
                   {

                       using (FileStream fs = new FileStream(fileFullName, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                       {
                           fs.Write(sdm.ContentBytes, 0, sdm.ContentBytes.Length);
                       }

                   }).GetAwaiter().OnCompleted(() =>
                   {

                       SaveDataModel sdm = new SaveDataModel
                       {
                           Content = fileFullName,
                           DataType = SaveDataType.File,
                           Guid = d.DataGuid,
                           Time = DateTime.Now,
                           SourceDeviceNickName = d.TargetDeviceNickName
                       };
                       _dataService.AddAsync(sdm);

                       NewMessageVisible = true;
                       IsDownLoadingVisible = false;

                   });

                }



            };

            this.receiveBussiness.ReceivingDataEvent += (ip) =>
            {
                IsDownLoadingVisible = true;
                if (DiscoveredDevices != null)
                {
                    DiscoveredDeviceModel find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
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
                    DiscoveredDeviceModel find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
                    if (find != null)
                    {
                        find.IsDownLoading = false;
                        find.DownloadProcess = 0;
                    }
                }
            };

            this.receiveBussiness.ReceivingProcessEvent += (ip, process) =>
            {
                if (DiscoveredDevices != null)
                {
                    DiscoveredDeviceModel find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
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
                    DiscoveredDeviceModel find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
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
                    DiscoveredDeviceModel find = DiscoveredDevices.FirstOrDefault(x => x.Ip == ip);
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
        public async Task Init()
        {
            IsBusy = true;
            FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
            UserName = userRes.Data.Name;
            DeviceNickName = userRes.Data.DeviceNickName;
            IsBusy = false;
            deviceDiscover();
        }

        [RelayCommand]
        public void Search()
        {
            deviceDiscover();
        }


        [RelayCommand]
        public async Task GotoList()
        {
            NewMessageVisible = false;

            await _navigationService.NavigationToAsync(nameof(ListPage), null);


        }


        [RelayCommand]
        public async Task Share(DiscoveredDeviceModel model)
        {
            if (model.IsSendingData)
            {

                await _dialogService.DisplayAlert("Warning", "Sorry, the file is being uploaded. Please try again after the upload is completed!", "Ok");
                return;
            }


            NavigationParameter parameter = new NavigationParameter();
            parameter.Add("data", model);
            await _dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, null);
        }

        [RelayCommand]
        public async Task GotoSettingPage()
        {


            await _navigationService.NavigationToAsync(nameof(SettingPage), null);

        }

        /// <summary>
        /// 设备发现
        /// </summary>
        private void deviceDiscover()
        {
            DiscoveredDevices.Clear();
            IsBusy = true;
            Task.Run(async () =>
            {
                await sendDataBussiness.DeviceDiscover();
            }).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(async () =>
            {
                await Task.Delay(1000);
                IsBusy = false;

                if (DiscoveredDevices.Count == 0)
                {

                    await Application.Current.MainPage.DisplayAlert("warning", "No connectable devices found! ", "Ok");
                }
            });
        }

    }
}
