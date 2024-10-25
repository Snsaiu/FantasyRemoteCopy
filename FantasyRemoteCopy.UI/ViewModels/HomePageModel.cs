using System.Collections.ObjectModel;
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
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;
using FantasyResultModel;
using H.NotifyIcon;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class HomePageModel : ViewModelBase, IPageKeep, INavigationAware
{
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
        IPortCheckable portCheckable,
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
        _portCheckable = portCheckable;
        _navigationService = navigationService;
        DiscoveredDevices = [];

        DiscoveredDevices.CollectionChanged += (s, e) => { SendCommand.NotifyCanExecuteChanged(); };

        Task.Run(() => Task.FromResult(SetReceive()));
    }

    [ObservableProperty] private InformationModel? informationModel = null;

    public void OnNavigatedTo(string source, INavigationParameter parameter)
    {
        if (parameter is null)
        {
            InformationModel = null;
            SendCommand.NotifyCanExecuteChanged();
            return;
        }

        var obj = parameter.Get("data");
        if (obj is not InformationModel information)
            throw new NullReferenceException();

        this.InformationModel = information;
        SendCommand.NotifyCanExecuteChanged();

        //if (obj is not SendTextModel text) return;

        //var device = DiscoveredDevices.FirstOrDefault(x => x.Flag == text.TargetFlag);
        //if (device is null)
        //    throw new NullReferenceException();

        //Task.Run(async () =>
        //{
        //    try
        //    {
        //        device.WorkState = WorkState.Sending;
        //        await _tcpSendTextBase.SendAsync(text, ReportProgress(true),
        //            device.CancellationTokenSource.Token);
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        device.WorkState = WorkState.None;
        //    }
        //});
    }

    public void OnNavigatedFrom(string source, INavigationParameter parameter)
    {
    }


    public bool Keep { get; set; } = true;


    [RelayCommand]
    public void ShowHideWindow()
    {
        var window = Application.Current?.MainPage?.Window;
        if (window == null) return;

        if (isWindowVisible)
            window.Hide();
        else
            window.Show();

        isWindowVisible = !isWindowVisible;
    }

    [RelayCommand]
    public async Task Init()
    {
        ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
        UserName = userRes.Data.Name;
        DeviceNickName = userRes.Data.DeviceNickName;
    }


    private void InitData()
    {
        DiscoveredDevices.Add(new DiscoveredDeviceModel()
            { Flag = "192.168.1.1", DeviceName = "my pc", NickName = "dfdf", SystemType = SystemType.Windows });
        DiscoveredDevices.Add(new DiscoveredDeviceModel()
            { Flag = "192.168.1.2", DeviceName = "my pc", NickName = "我的mac", SystemType = SystemType.MacOS });
    }

    private async Task SetReceive()
    {
        try
        {
            IsBusy = true;

            var localIp = await _deviceLocalIpBase.GetLocalIpAsync();
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
        var thread = new Thread(() =>
        {
            _ = _localNetDeviceDiscoveryBase.ReceiveAsync(x =>
            {
                if (localIp == x.Flag)
                    return;

                if (x.Name != UserName)
                    return;

                var joinRequestModel = new JoinMessageModel(_systemType.System, _deviceType.Device,
                    localIp, DeviceNickName, x.Flag, x.Name);
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
                    logger.LogInformation("接收到要加入的设备{0}", JsonConvert.SerializeObject(x));
                    if (x.Name != UserName)
                        return;

                    if (DiscoveredDevices.Any(y => y.Flag == x.Flag)) return;
                    logger.LogInformation("加入设备{0}", JsonConvert.SerializeObject(x));
                    DiscoveredDevices.Add(x);
                }, default);
            })
            { IsBackground = true };
        thread.Start();
    }

    private void StartTcpListener()
    {
        var thread = new Thread(() =>
            {
                _ = _tcpLoopListenContentBase.ReceiveAsync(CheckPortEnable, ReportProgress(false),
                    _cancelDownloadTokenSource.Token);
            })
            { IsBackground = true };
        thread.Start();
    }

    private async void CheckPortEnable(TransformResultModel<string> data)
    {
        try
        {
            var localIp = await _deviceLocalIpBase.GetLocalIpAsync();
            if (data.Result.Contains("portcheck_"))
            {
                var port = data.Result.Replace("portcheck_", "");
                this.logger.LogInformation($"端口{port}是否可用");
                var checkResult = await _portCheckable.IsPortInUse(int.Parse(port));
                this.logger.LogInformation($"端口{port}可用状态为 {checkResult}");
                var sendModel = new SendTextModel(localIp, data.Flag, checkResult ? "1" : "0");
                this.logger.LogInformation($"端口可用状态信息发送给{data.Flag}");
                await this._tcpSendTextBase.SendAsync(sendModel, null, default);
            }
            else
            {
                this.logger.LogInformation($"发送方接收回调方{data.Flag}端口可用情况，接收方端口可用情况为 {data.Result}");
                //端口不可用，进行累加
                if (data.Result == "0")
                {
                    int port = int.Parse(data.Result)+1;
                    this.logger.LogInformation($"接收方{data.Flag}对于{data.Result} 端口无法使用，所以向接收方再次发送{port}端口是否可用");
                    var portCheckMessage = new SendTextModel(localIp, data.Flag ?? throw new NullReferenceException(),
                        $"portcheck_{port}");
                    await _tcpSendTextBase.SendAsync(portCheckMessage, null, default);
                }
                else if (data.Result == "1")
                {
                    this.logger.LogInformation($"接收方{data.Flag}对于{data.Result}端口可用，使用https进行数据传输");
                    // 使用https发送数据
                }
            }
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    private void SaveDataToLocalDB(TransformResultModel<string> data)
    {
        var saveDataModel = new SaveDataModel
        {
            DataType = data.SendType,
            Content = data.Result,
            Time = DateTime.Now
        };

        var model = DiscoveredDevices.FirstOrDefault(x => x.Flag == data.Flag);
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

        var parameter = new NavigationParameter();
        parameter.Add("data", model);
        return _dialogService.ShowPopUpDialogAsync(nameof(SendTypeDialog), parameter, x =>
        {
            if (!x.Success)
                return;
            switch (x.Data)
            {
                case SendFileModel fileModel:
                    SendFile(fileModel);
                    break;
                case List<SendFileModel> fileModels:
                    SendCompressFile(fileModels);
                    break;
                case SendType and SendType.Text:
                    _navigationService.NavigationToAsync(nameof(TextInputPage), parameter);
                    break;
                case SendFolderModel folderModel:
                    SendCompressFile(folderModel);
                    break;
                default:
                    throw new NotImplementedException();
            }
        });
    }


    private void SendCompressFile(IEnumerable<SendFileModel> files)
    {
        Task.Run(async () =>
        {
            var first = files.First();

            var device =
                DiscoveredDevices.FirstOrDefault(y => y.Flag == first.TargetFlag);
            if (device is null)
                throw new NullReferenceException();
            try
            {
                var sendCompressFileModel =
                    new SendCompressFileModel(first.Flag, first.TargetFlag, files.Select(x => x.FileFullPath));

                device.WorkState = WorkState.Sending;
                await _tcpSendFileBase.SendAsync(sendCompressFileModel, ReportProgress(true),
                    device.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                device.WorkState = WorkState.None;
            }
        });
    }

    private void SendCompressFile(SendFolderModel folderModel)
    {
        Task.Run(async () =>
        {
            var device =
                DiscoveredDevices.FirstOrDefault(y => y.Flag == folderModel.TargetFlag);
            if (device is null)
                throw new NullReferenceException();
            try
            {
                var sendCompressFileModel = new SendCompressFileModel(folderModel);

                device.WorkState = WorkState.Sending;
                await _tcpSendFileBase.SendAsync(sendCompressFileModel, ReportProgress(true),
                    device.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                device.WorkState = WorkState.None;
            }
        });
    }

    private void SendFile(SendFileModel fileModel)
    {
        Task.Run(async () =>
        {
            var device =
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

    [RelayCommand]
    public Task GotoSettingPage()
    {
        return _navigationService.NavigationToAsync(nameof(SettingPage), null);
    }

    private bool CanSend()
    {
        return DiscoveredDevices.Any(x => x.IsChecked) && InformationModel is not null;
    }

    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task Send()
    {
        var localIp = await _deviceLocalIpBase.GetLocalIpAsync();
        // 首先向目标电脑发送一个端口用于检查是否可以使用该端口
        foreach (var item in DiscoveredDevices)
        {
            var portCheckMessage = new SendTextModel(localIp, item.Flag ?? throw new NullReferenceException(),
                $"portcheck_{5005}");
            await _tcpSendTextBase.SendAsync(portCheckMessage, null, default);
        }
    }

    [RelayCommand]
    private Task GoText()
    {
        return _navigationService.NavigationToAsync(nameof(TextInputPage), null);
    }


    /// <summary>
    ///     设备发现
    /// </summary>
    private async Task DeviceDiscoverAsync()
    {
        try
        {
            IsBusy = true;
            await Task.Yield();
            DiscoveredDevices.Clear();
            var localIp = await _deviceLocalIpBase.GetLocalIpAsync();
            logger.LogInformation("发现本地ip:{0}", localIp);

            var devices = _localIpScannerBase.GetDevicesAsync(default);

            await foreach (var device in devices)
            {
                logger.LogInformation("通过设备发现扫描到的ip:{0}", device.Flag);

                await _localNetInviteDeviceBase.SendAsync(new DeviceDiscoveryMessage(UserName, localIp, device.Flag) ??
                                                          throw new NullReferenceException(), default);
            }
        }
        finally
        {
            IsBusy = false;
            await Task.Yield();
        }
    }


    private IProgress<ProgressValueModel> ReportProgress(bool isSendModel)
    {
        Progress<ProgressValueModel> progress = new(x =>
        {
            var flag = isSendModel
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
    private readonly IPortCheckable _portCheckable;
    private readonly INavigationService _navigationService;
    private readonly CancellationTokenSource _cancelDownloadTokenSource = new();

    #endregion

    #region NotifyProperties

    [ObservableProperty] private bool isDownLoadingVisible;

    [ObservableProperty] private bool newMessageVisible;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] private string userName = string.Empty;

    [ObservableProperty] private ItemsChangeObservableCollection<DiscoveredDeviceModel> discoveredDevices;

    [ObservableProperty] private string deviceNickName = string.Empty;

    #endregion
}