using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyMvvm.FantasyRegionManager;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

using Newtonsoft.Json;

using System.Collections.ObjectModel;
using System.Text;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using UserInfo = FantasyRemoteCopy.UI.Models.UserInfo;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class HomePageModel : FantasyPageModelBase, IPageKeep
    {
        private readonly IDialogService _dialogService;
        private readonly LocalNetDeviceDiscoveryBase _localNetDeviceDiscoveryBase;
        private readonly LocalNetInviteDeviceBase _localNetInviteDeviceBase;
        private readonly DeviceLocalIpBase _deviceLocalIpBase;
        private readonly LocalIpScannerBase _localIpScannerBase;

        private readonly IRegionManager _regionManager;
        private readonly LocalNetJoinRequestBase _localNetJoinRequestBase;
        private readonly LocalNetJoinProcessBase _localNetJoinProcessBase;
        private readonly ISystemType _systemType;
        private readonly IDeviceType _deviceType;

        private readonly INavigationService _navigationService;

        private LocalNetInviteMessage? _localNetInviteMessage = null;
        
        public HomePageModel(IUserService userService, 
            ISaveDataService dataService,
            IFileSaveLocation fileSaveLocation, 
            IDialogService dialogService, 
            LocalNetDeviceDiscoveryBase localNetDeviceDiscoveryBase,
            LocalNetInviteDeviceBase localNetInviteDeviceBase,
            DeviceLocalIpBase deviceLocalIpBase,
            LocalIpScannerBase localIpScannerBase,
            IRegionManager regionManager,
            LocalNetJoinRequestBase localNetJoinRequestBase,
            LocalNetJoinProcessBase localNetJoinProcessBase,
            ISystemType systemType,
            IDeviceType deviceType, 
            INavigationService navigationService)
        {
            this.userService = userService;
            _dataService = dataService;
            _fileSaveLocation = fileSaveLocation;
            _dialogService = dialogService;
            _localNetDeviceDiscoveryBase = localNetDeviceDiscoveryBase;
            _localNetInviteDeviceBase = localNetInviteDeviceBase;
            _deviceLocalIpBase = deviceLocalIpBase;
            _localIpScannerBase = localIpScannerBase;
            _regionManager = regionManager;
            _localNetJoinRequestBase = localNetJoinRequestBase;
            _localNetJoinProcessBase = localNetJoinProcessBase;
            _systemType = systemType;
            _deviceType = deviceType;
            _navigationService = navigationService;
            DiscoveredDevices = [];
            
            Task.Run(() => Task.FromResult( Init()));
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
        private readonly ISaveDataService _dataService;
        private readonly IFileSaveLocation _fileSaveLocation;

        public bool Keep { get; set; } = true;
        
        private async Task Init()
        {
            IsBusy = true;
            FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
            UserName = userRes.Data.Name;
            DeviceNickName = userRes.Data.DeviceNickName;

            var localIp = await this._deviceLocalIpBase.GetLocalIpAsync();
            
           
            _localNetInviteMessage = new LocalNetInviteMessage(UserName, localIp);

            
            //设备发现 ，当有新的设备加入的时候产生回调
           this.StartDiscovery(localIp);
           this.StartJoin();
            
            IsBusy = false;
            await DeviceDiscoverAsync(false);
        }

        private void StartDiscovery(string localIp)
        {
            var thread = new Thread(() =>
            {
                 this._localNetDeviceDiscoveryBase.ReceiveAsync(x =>
                {
                    var joinRequestModel = new JoinMessageModel(this._systemType.System,this._deviceType.Device, localIp,DeviceNickName,x.Ip);
                    // 发送加入请求
                    this._localNetJoinRequestBase.SendAsync(joinRequestModel);
                });
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void StartJoin()
        {
            var thread = new Thread(() =>
            {
                this._localNetJoinProcessBase.ReceiveAsync(x =>
                {

                });

            }) { IsBackground = true };
            thread.Start();
        }

        [RelayCommand]
        public Task Search()
        {
            return DeviceDiscoverAsync(true);
        }


        [RelayCommand]
        public Task GotoList()
        {
            NewMessageVisible = false;
            return _navigationService.NavigationToAsync(nameof(ListPage), null);
        }


        [RelayCommand]
        public Task Share(DiscoveredDeviceModel model)
        {
            if (model.IsSendingData)
                return _dialogService.DisplayAlert("Warning", "Sorry, the file is being uploaded. Please try again after the upload is completed!", "Ok");

            NavigationParameter parameter = new NavigationParameter();
            parameter.Add("data", model);
            return _dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, null);
        }

        [RelayCommand]
        public Task GotoSettingPage()
        {
            return _navigationService.NavigationToAsync(nameof(SettingPage), null);
        }

        /// <summary>
        /// 设备发现
        /// </summary>
        private async Task DeviceDiscoverAsync(bool showWarning)
        {
            try
            {
                var cancellationToken = new CancellationToken();
                
                var devices = this._localIpScannerBase.GetDevicesAsync(cancellationToken);

                await foreach (var device in devices)
                {
                    await this._localNetInviteDeviceBase.SendAsync(_localNetInviteMessage ??
                                                                     throw new NullReferenceException());
                }
                
                DiscoveredDevices.Clear();
                IsBusy = true;
            }
            finally
            {
                IsBusy = false;
            }
            
            if (DiscoveredDevices.Count == 0 && showWarning)
            {
                Application.Current?.MainPage?.DisplayAlert("warning", "No connectable devices found! ", "Ok");
            }

        }

    }
}
