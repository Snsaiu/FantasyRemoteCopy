using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

using H.NotifyIcon;

using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FantasyRemoteCopy.UI.ViewModels
{
    public partial class HomePageModel : ViewModelBase, IPageKeep, INavigationAware
    {
        #region Fields

        private bool isWindowVisible = true;
        private readonly IUserService userService;
        private readonly ISaveDataService _dataService;
        private readonly IDialogService _dialogService;
        private readonly LocalNetDeviceDiscoveryBase _localNetDeviceDiscoveryBase;
        private readonly LocalNetInviteDeviceBase _localNetInviteDeviceBase;
        private readonly DeviceLocalIpBase _deviceLocalIpBase;
        private readonly LocalIpScannerBase _localIpScannerBase;
        private readonly LocalNetJoinRequestBase _localNetJoinRequestBase;
        private readonly LocalNetJoinProcessBase _localNetJoinProcessBase;
        private readonly TcpLoopListenContentBase _tcpLoopListenContentBase;
        private readonly TcpSendFileBase _tcpSendFileBase;
        private readonly TcpSendTextBase _tcpSendTextBase;
        private readonly ISystemType _systemType;
        private readonly ILogger<HomePageModel> logger;
        private readonly IDeviceType _deviceType;
        private readonly INavigationService _navigationService;
        private readonly CancellationTokenSource _cancelDownloadTokenSource = new CancellationTokenSource();

        #endregion

        #region NotifyProperties

        [ObservableProperty] private bool isDownLoadingVisible;

        [ObservableProperty] private bool newMessageVisible;

        [ObservableProperty] private bool isBusy;

        [ObservableProperty] private string userName = string.Empty;

        [ObservableProperty] private ObservableCollection<DiscoveredDeviceModel> discoveredDevices;

        [ObservableProperty] private string deviceNickName = String.Empty;

        #endregion

        public HomePageModel(IUserService userService,
            ISaveDataService dataService,
            IDialogService dialogService,
            LocalNetDeviceDiscoveryBase localNetDeviceDiscoveryBase,
            LocalNetInviteDeviceBase localNetInviteDeviceBase,
            DeviceLocalIpBase deviceLocalIpBase,
            LocalIpScannerBase localIpScannerBase,
            LocalNetJoinRequestBase localNetJoinRequestBase,
            LocalNetJoinProcessBase localNetJoinProcessBase,
            TcpLoopListenContentBase tcpLoopListenContentBase,
            TcpSendFileBase tcpSendFileBase,
            TcpSendTextBase tcpSendTextBase,
            ISystemType systemType,
            ILogger<HomePageModel> logger,
            IDeviceType deviceType,
            INavigationService navigationService)
        {
            this.userService = userService;
            _dataService = dataService;
            _dialogService = dialogService;
            _localNetDeviceDiscoveryBase = localNetDeviceDiscoveryBase;
            _localNetInviteDeviceBase = localNetInviteDeviceBase;
            _deviceLocalIpBase = deviceLocalIpBase;
            _localIpScannerBase = localIpScannerBase;
            _localNetJoinRequestBase = localNetJoinRequestBase;
            _localNetJoinProcessBase = localNetJoinProcessBase;
            _tcpLoopListenContentBase = tcpLoopListenContentBase;
            _tcpSendFileBase = tcpSendFileBase;
            _tcpSendTextBase = tcpSendTextBase;
            _systemType = systemType;
            this.logger = logger;
            _deviceType = deviceType;
            _navigationService = navigationService;
            DiscoveredDevices = [];
            Task.Run(() => Task.FromResult(SetReceive()));
        }


        public bool Keep { get; set; } = true;


        [RelayCommand]
        public void ShowHideWindow()
        {
            Window? window = Application.Current?.MainPage?.Window;
            if (window == null)
            {
                return;
            }

            if (isWindowVisible)
            {
                window.Hide();
            }
            else
            {
                window.Show();
            }

            isWindowVisible = !isWindowVisible;
        }

        [RelayCommand]
        public async Task Init()
        {
            FantasyResultModel.ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
            UserName = userRes.Data.Name;
            DeviceNickName = userRes.Data.DeviceNickName;
        }

        private async Task SetReceive()
        {
            try
            {
                IsBusy = true;
              
                string localIp = await _deviceLocalIpBase.GetLocalIpAsync();
                //设备发现 ，当有新的设备加入的时候产生回调
                StartDiscovery(localIp);
                StartJoin();
                StartTcpListener();
                await DeviceDiscoverAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void StartDiscovery(string localIp)
        {
            Thread thread = new Thread(() =>
            {
                _ = _localNetDeviceDiscoveryBase.ReceiveAsync(x =>
                {
                    if (localIp == x.Flag)
                        return;
                    
                    if(x.Name!=UserName)
                        return;
                    
                    var joinRequestModel = new JoinMessageModel(_systemType.System, _deviceType.Device,
                        localIp, DeviceNickName, x.Flag,x.Name);
                    // 发送加入请求
                    _localNetJoinRequestBase.SendAsync(joinRequestModel, default);
                }, default);
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
                    _ = _localNetJoinProcessBase.ReceiveAsync(x =>
                    {
                        logger.LogInformation("接收到要加入的设备{0}",JsonConvert.SerializeObject(x));
                        if(x.Name!=UserName)
                            return;
                        
                        if (DiscoveredDevices.Any(y => y.Flag == x.Flag))
                        {
                            return;
                        }
                        logger.LogInformation("加入设备{0}",JsonConvert.SerializeObject(x));

                        DiscoveredDevices.Add(x);
                    }, default);
                })
            { IsBackground = true };
            thread.Start();
        }

        private void StartTcpListener()
        {
            Thread thread = new Thread(() =>
                {
                    _ = _tcpLoopListenContentBase.ReceiveAsync(SaveDataToLocalDB, ReportProgress(false),
                        _cancelDownloadTokenSource.Token);
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
            model.WorkState = WorkState.None;
            NewMessageVisible = true;
            IsDownLoadingVisible = false;
            saveDataModel.SourceDeviceNickName = model.NickName ?? string.Empty;
            saveDataModel.Guid = Guid.NewGuid().ToString();
            _dataService.AddAsync(saveDataModel);
        }

        [RelayCommand]
        public Task Search()
        {
            return DeviceDiscoverAsync();
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
            if (model.WorkState == WorkState.Sending)
                return _dialogService.DisplayAlert("Warning",
                    "Sorry, the file is being uploaded. Please try again after the upload is completed!", "Ok");

            NavigationParameter parameter = new NavigationParameter();
            parameter.Add("data", model);
            return _dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, x =>
            {
                if (!x.Success)
                    return;
                if (x.Data is SendFileModel fileModel)
                {
                    Task.Run(async () =>
                    {
                        DiscoveredDeviceModel? device =
                            DiscoveredDevices.FirstOrDefault(y => y.Flag == fileModel.TargetFlag);
                        if (device is null)
                            throw new NullReferenceException();
                        try
                        {
                            device.WorkState = WorkState.Sending;
                            await _tcpSendFileBase.SendAsync(fileModel, ReportProgress(true),
                                device.CancellationTokenSource.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            device.WorkState = WorkState.None;
                        }
                    });
                }
                else if (x.Data is SendType and SendType.Text)
                {
                    _navigationService.NavigationToAsync(nameof(TextInputPage), parameter);
                }
                else
                {
                    throw new NotImplementedException();
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
        private async Task DeviceDiscoverAsync()
        {
            try
            {
                IsBusy = true;
                DiscoveredDevices.Clear();
                var localIp = await _deviceLocalIpBase.GetLocalIpAsync();
                logger.LogInformation("发现本地ip:{0}",localIp);
              
                var devices = _localIpScannerBase.GetDevicesAsync(default);

                await foreach (ScanDevice device in devices)
                {
                    logger.LogInformation("通过设备发现扫描到的ip:{0}",device.Flag);
                    
                    await _localNetInviteDeviceBase.SendAsync(new DeviceDiscoveryMessage(UserName, localIp, device.Flag) ??
                                                              throw new NullReferenceException(), default);
                    
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
            if (obj is not SendTextModel text) return;

            DiscoveredDeviceModel? device = DiscoveredDevices.FirstOrDefault(x => x.Flag == text.TargetFlag);
            if (device is null)
                throw new NullReferenceException();

            Task.Run(async () =>
            {
                try
                {
                    device.WorkState = WorkState.Sending;
                    await _tcpSendTextBase.SendAsync(text, ReportProgress(true),
                        device.CancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    device.WorkState = WorkState.None;
                }
            });
        }


        private IProgress<ProgressValueModel> ReportProgress(bool isSendModel)
        {
            Progress<ProgressValueModel> progress = new Progress<ProgressValueModel>(x =>
            {
                DiscoveredDeviceModel? flag = isSendModel
                    ? DiscoveredDevices.FirstOrDefault(y => y.Flag == x.TargetFlag)
                    : DiscoveredDevices.FirstOrDefault(y => y.Flag == x.Flag);
                if (flag is null)
                    return;
                if (isSendModel)
                {
                    if (x.Progress >= 1)
                    {
                        flag.WorkState = WorkState.None;
                        flag.Progress = 0;
                    }
                    else
                    {
                        flag.WorkState = WorkState.Sending;
                    }
                }
                else
                {
                    if (x.Progress >= 1)
                    {
                        flag.WorkState = WorkState.None;
                        flag.Progress = 0;
                    }
                    else
                    {
                        flag.WorkState = WorkState.Downloading;
                    }
                }

                flag.Progress = x.Progress;
            });
            return progress;
        }

        public void OnNavigatedFrom(string source, INavigationParameter parameter)
        {
        }
    }
}