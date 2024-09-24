using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyMvvm.FantasyRegionManager;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

using H.NotifyIcon;

using System.Collections.ObjectModel;

using UserInfo = FantasyRemoteCopy.UI.Models.UserInfo;

namespace FantasyRemoteCopy.UI.ViewModels
{

    public partial class HomePageModel : FantasyPageModelBase, IPageKeep, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly LocalNetDeviceDiscoveryBase _localNetDeviceDiscoveryBase;
        private readonly LocalNetInviteDeviceBase _localNetInviteDeviceBase;
        private readonly DeviceLocalIpBase _deviceLocalIpBase;
        private readonly LocalIpScannerBase _localIpScannerBase;

        private readonly IRegionManager _regionManager;
        private readonly LocalNetJoinRequestBase _localNetJoinRequestBase;
        private readonly LocalNetJoinProcessBase _localNetJoinProcessBase;
        private readonly TcpLoopListenContentBase _tcpLoopListenContentBase;
        private readonly TcpSendFileBase _tcpSendFileBase;
        private readonly TcpSendTextBase _tcpSendTextBase;
        private readonly ISystemType _systemType;
        private readonly IDeviceType _deviceType;

        private readonly INavigationService _navigationService;

        private DeviceDiscoveryMessage? _localNetInviteMessage = null;

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
            TcpLoopListenContentBase tcpLoopListenContentBase,
            TcpSendFileBase tcpSendFileBase,
            TcpSendTextBase tcpSendTextBase,
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
            _tcpLoopListenContentBase = tcpLoopListenContentBase;
            _tcpSendFileBase = tcpSendFileBase;
            _tcpSendTextBase = tcpSendTextBase;
            _systemType = systemType;
            _deviceType = deviceType;
            _navigationService = navigationService;
            DiscoveredDevices = [];

            Task.Run(() => Task.FromResult(Init()));
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

        private bool IsWindowVisible { get; set; } = true;

        private readonly IUserService userService;
        private readonly ISaveDataService _dataService;
        private readonly IFileSaveLocation _fileSaveLocation;

        public bool Keep { get; set; } = true;


        [RelayCommand]
        public void ShowHideWindow()
        {
            Window? window = Application.Current?.MainPage?.Window;
            if (window == null)
            {
                return;
            }

            if (IsWindowVisible)
            {
                window.Hide();
            }
            else
            {
                window.Show();
            }
            IsWindowVisible = !IsWindowVisible;
        }
        private async Task Init()
        {
            IsBusy = true;
            FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
            UserName = userRes.Data.Name;
            DeviceNickName = userRes.Data.DeviceNickName;

            string localIp = await _deviceLocalIpBase.GetLocalIpAsync();

            _localNetInviteMessage = new DeviceDiscoveryMessage(UserName, localIp);

            //设备发现 ，当有新的设备加入的时候产生回调
            StartDiscovery(localIp);
            StartJoin();
            StartTcpListener();

            IsBusy = false;
            await DeviceDiscoverAsync(false);
        }

        private void StartDiscovery(string localIp)
        {
            Thread thread = new Thread(() =>
            {
                _ = _localNetDeviceDiscoveryBase.ReceiveAsync(x =>
                {
                    if (localIp == x.Flag)
                        return;

                    JoinMessageModel joinRequestModel = new JoinMessageModel(_systemType.System, _deviceType.Device, localIp, DeviceNickName, x.Flag);
                    // 发送加入请求
                    _localNetJoinRequestBase.SendAsync(joinRequestModel);
                });
            })
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void StartJoin()
        {
            Thread thread = new Thread(() =>
            {
                _ = _localNetJoinProcessBase.ReceiveAsync(x =>
                {
                    if (DiscoveredDevices.Any(y => y.Flag == x.Flag))
                    {
                        return;
                    }

                    DiscoveredDevices.Add(x);

                });

            })
            { IsBackground = true };
            thread.Start();
        }

        private void StartTcpListener()
        {
            Thread thread = new Thread(() =>
            {
                _ = _tcpLoopListenContentBase.ReceiveAsync(SaveDataToLocalDB, ReportProgress(false));
            })
            { IsBackground = true };
            thread.Start();
        }

        private void SaveDataToLocalDB(TransformResultModel<string> data)
        {
            SaveDataModel saveDataModel = new SaveDataModel
            {
                DataType = data.SendType,
                Content = data.Result,
                Time = DateTime.Now
            };

            DiscoveredDeviceModel? model = DiscoveredDevices.FirstOrDefault(x => x.Flag == data.Flag);
            if (model is null)
                throw new NullReferenceException();
            model.IsDownLoading = false;
            saveDataModel.SourceDeviceNickName = model.NickName;
            saveDataModel.Guid = Guid.NewGuid().ToString();
            _dataService.AddAsync(saveDataModel);
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
            return _dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, x =>
            {
                if (!x.Success)
                    return;
                if (x.Data is SendFileModel fileModel)
                {
                    Task.Run(() => _tcpSendFileBase.SendAsync(fileModel, ReportProgress(true)));
                }
            });
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
                IsBusy = true;
                DiscoveredDevices.Clear();

                CancellationToken cancellationToken = new CancellationToken();

                IAsyncEnumerable<ScanDevice> devices = _localIpScannerBase.GetDevicesAsync(cancellationToken);

                await foreach (ScanDevice device in devices)
                {
                    await _localNetInviteDeviceBase.SendAsync(_localNetInviteMessage ??
                                                                     throw new NullReferenceException());
                }
            }
            finally
            {
                IsBusy = false;
            }

        }


        public void OnNavigatedTo(string source, INavigationParameter parameter)
        {
            if (parameter is null)
                return;
            object obj = parameter.Get("data");
            if (obj is SendTextModel text)
                Task.Run(() => _tcpSendTextBase.SendAsync(text, ReportProgress(true)));

        }


        private IProgress<ProgressValueModel> ReportProgress(bool isSendModel)
        {
            Progress<ProgressValueModel> progress = new Progress<ProgressValueModel>(x =>
            {
                DiscoveredDeviceModel? flag = DiscoveredDevices.FirstOrDefault(y => y.Flag == x.Flag);
                if (flag is null)
                    return;
                if (isSendModel)
                {
                    flag.IsSendingData = true;
                    flag.IsDownLoading = false;
                }
                else
                {
                    flag.IsSendingData = false;
                    flag.IsDownLoading = true;
                    flag.DownloadProcess = x.Progress;
                }

            });
            return progress;
        }

        public void OnNavigatedFrom(string source, INavigationParameter parameter)
        {

        }
    }
}
