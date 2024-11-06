using System.Collections.ObjectModel;
using System.Net;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Extensions;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.HttpsTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;
using FantasyResultModel;
using H.NotifyIcon;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class HomePageModel : ViewModelBase, IPageKeep, INavigationAware
{
    [ObservableProperty] private InformationModel? informationModel;

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


        Task.Run(() => Task.FromResult(SetReceive()));
    }

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

        InformationModel = information;
        SendCommand.NotifyCanExecuteChanged();
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
        //InitData();
    }


    private void InitData()
    {
        DiscoveredDevices.Add(new DiscoveredDeviceModel
            { Flag = "192.168.1.1", DeviceName = "my pc", NickName = "dfdf", SystemType = SystemType.Windows });
        DiscoveredDevices.Add(new DiscoveredDeviceModel
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
                _ = _tcpLoopListenContentBase.ReceiveAsync(CheckPortEnable, IPAddress.Any, ConstParams.TCP_PORT,
                    ReportProgress(false),
                    _cancelDownloadTokenSource.Token);
            })
            { IsBackground = true };
        thread.Start();
    }

    [RelayCommand]
    private async Task CloseTransformAsync(DiscoveredDeviceModel device)
    {
        if (device.TransmissionTasks.FirstOrDefault() is { } receiveTask)
        {
            var cancelCodeWord = CodeWordModel.Create(receiveTask.TaskGuid, CodeWordType.CancelTransmission,
                receiveTask.Flag,
                receiveTask.TargetFlag, receiveTask.Port, receiveTask.SendType);

            this.logger.LogInformation(
                $"请求 {cancelCodeWord.Flag} 与 {cancelCodeWord.TargetFlag} 通过端口 {cancelCodeWord.Port} 的数据传输人工中断");

            await this._tcpSendTextBase.SendAsync(
                new SendTextModel(receiveTask.Flag, receiveTask.TargetFlag, cancelCodeWord.ToJson(),
                    ConstParams.TCP_PORT), null, default);

            receiveTask.CancellationTokenSource.Cancel();
        }

        device.Progress = 0;
        device.WorkState = WorkState.None;
    }


    private async void CheckPortEnable(TransformResultModel<string> data)
    {
        var localIp = await _deviceLocalIpBase.GetLocalIpAsync();

        var codeWord = data.Result.ToObject<CodeWordModel>();
        if (codeWord.Type == CodeWordType.CheckingPort)
        {
            var port = codeWord.Port;
            logger.LogInformation($"端口{port}是否可用");
            var checkResult = await _portCheckable.IsPortInUse(port);
            logger.LogInformation($"端口{port}可用状态为 {checkResult}");
            var sendModel = new SendTextModel(localIp, data.Flag,
                !checkResult
                    ? CodeWordModel.Create(codeWord.TaskGuid, CodeWordType.CheckingPortCanUse, localIp, data.Flag, port,
                            codeWord.SendType)
                        .ToJson()
                    : CodeWordModel.Create(codeWord.TaskGuid, CodeWordType.CheckingPortCanNotUse, localIp, data.Flag,
                        port,
                        codeWord.SendType).ToJson(),
                ConstParams.TCP_PORT);
            logger.LogInformation($"端口可用状态信息发送给{data.Flag}");
            if (!checkResult)
            {
                // 开始监听
                var cancelTokenSource = new CancellationTokenSource();
                var receiveDevice = DiscoveredDevices.FirstOrDefault(x => x.Flag == data.Flag);
                if (receiveDevice is null)
                {
                    logger.LogInformation($"设备列表中未发现端口为{data.Flag},取消监听");
                    return;
                }

                if (receiveDevice.TransmissionTasks.Any(
                        x => x.TaskGuid == codeWord.TaskGuid))
                    logger.LogInformation($"{data.Flag}中已经包含了 {data.Flag}-{port} 的任务");
                else
                    receiveDevice.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.Type,
                        localIp, codeWord.Flag, codeWord.Port, codeWord.SendType, cancelTokenSource));

                _tcpLoopListenContentBase.ReceiveAsync(result =>
                {
                    // 保存到数据库
                    SaveDataToLocalDB(result);
                    if (receiveDevice.TryGetTransmissionTask(codeWord.TaskGuid,
                            out var v))
                    {
                        v?.CancellationTokenSource.Cancel();
                        receiveDevice.RemoveTransmissionTask(codeWord.TaskGuid);
                    }
                }, IPAddress.Parse(data.Flag), port, ReportProgress(false), cancelTokenSource.Token);
            }

            await _tcpSendTextBase.SendAsync(sendModel, null, default);
        }
        else if (codeWord.Type == CodeWordType.CancelTransmission)
        {
            foreach (var device in DiscoveredDevices)
            {
                if (device.TryGetTransmissionTask(codeWord.TaskGuid, out var task))
                {
                    task?.CancellationTokenSource?.Cancel();
                    device.Progress = 0;
                    device.WorkState = WorkState.None;
                    return;
                }
            }
        }
        else
        {
            logger.LogInformation($"发送方接收回调方{data.Flag}端口可用情况，接收方端口可用情况为 {codeWord.Type}");

            //端口不可用，进行累加
            if (codeWord.Type == CodeWordType.CheckingPortCanNotUse)
            {
                var port = codeWord.Port + 1;
                logger.LogInformation($"接收方{data.Flag}对于{codeWord.Port} 端口无法使用，所以向接收方再次发送{port}端口是否可用");
                var portCheckMessage = new SendTextModel(localIp,
                    data.Flag ?? throw new NullReferenceException(),
                    CodeWordModel.Create(codeWord.TaskGuid, CodeWordType.CheckingPort, localIp, data.Flag, port,
                            codeWord.SendType)
                        .ToJson()
                    , ConstParams.TCP_PORT);
                await _tcpSendTextBase.SendAsync(portCheckMessage, null, default);
            }
            else if (codeWord.Type == CodeWordType.CheckingPortCanUse)
            {
                var sendCancelTokenSource = new CancellationTokenSource();

                SenderAddTaskAndShowProgress(codeWord, sendCancelTokenSource);

                switch (InformationModel!.SendType)
                {
                    case SendType.Text:
                        _tcpSendTextBase.SendAsync(
                            new SendTextModel(localIp, data.Flag, InformationModel.Text, codeWord.Port), null,
                            sendCancelTokenSource.Token);
                        break;
                    case SendType.File:
                    case SendType.Folder:
                        SendFileAsync(data.Flag, codeWord.Port, sendCancelTokenSource.Token);
                        break;
                }
            }
        }
    }

    private void SenderAddTaskAndShowProgress(CodeWordModel codeWord, CancellationTokenSource cancelTokenSource)
    {
        var device = DiscoveredDevices.FirstOrDefault(x => x.Flag == codeWord.Flag);
        if (device is null) return;
        device.WorkState = WorkState.Sending;
        device.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.Type, codeWord.TargetFlag,
            codeWord.Flag, codeWord.Port, codeWord.SendType, codeWord.CancellationTokenSource));
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
    private async Task OpenFolderAsync()
    {
        var folerPicker = await FolderPicker.PickAsync(default);
        if (!folerPicker.IsSuccessful)
            return;

        InformationModel = new InformationModel
        {
            SendType = SendType.Folder,
            FolderPath = folerPicker.Folder.Path
        };
    }

    [RelayCommand]
    private async Task OpenFileAsync()
    {
        var files = await FilePicker.PickMultipleAsync();

        if (!files.Any())
            return;
        InformationModel = new InformationModel
        {
            SendType = SendType.File,
            Files = files.Select(x => x.FullPath)
        };
    }

    private async Task SendFileAsync(string targetIp, int port, CancellationToken token)
    {
        var localIp = await _deviceLocalIpBase.GetLocalIpAsync();

        try
        {
            SendFileModel sendfile;

            if (InformationModel!.SendType == SendType.Folder)
            {
                sendfile = new SendCompressFileModel(localIp, targetIp, InformationModel.FolderPath, port);
            }
            else if (InformationModel.SendType == SendType.File)
            {
                if (InformationModel.Files.Count() == 1)
                    sendfile = new SendFileModel(localIp, targetIp, InformationModel.Files.First(), port);
                else
                    sendfile = new SendCompressFileModel(localIp, targetIp, InformationModel.Files, port);
            }
            else
            {
                throw new NotSupportedException();
            }

            // device.WorkState = WorkState.Sending;
            await _tcpSendFileBase.SendAsync(sendfile, ReportProgress(true), token);
            //   device.CancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            //  device.WorkState = WorkState.None;
        }
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
            if (!item.IsChecked)
                continue;

            var codeWord = CodeWordModel.Create(Guid.NewGuid().ToString("N"), CodeWordType.CheckingPort, localIp,
                item.Flag, 5005,
                InformationModel.SendType);


            var portCheckMessage = new SendTextModel(localIp, item.Flag ?? throw new NullReferenceException(),
                codeWord.ToJson(), ConstParams.TCP_PORT);
            await _tcpSendTextBase.SendAsync(portCheckMessage, null, default);
        }
    }

    [RelayCommand]
    private Task GoText()
    {
        return _navigationService.NavigationToAsync(nameof(TextInputPage), null);
    }

    [RelayCommand]
    private void DeviceCheckChanged()
    {
        SendCommand.NotifyCanExecuteChanged();
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

            IAsyncEnumerable<ScanDevice> devices = _localIpScannerBase.GetDevicesAsync(default);

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
                logger.LogInformation($"发送数据到{flag.Flag} 进度为{flag.Progress}");
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
                logger.LogInformation($"接收数据{flag.Flag} 进度为{flag.Progress}");
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
    private readonly HttpsSendTextBase _httpsSendTextBase;
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

    [ObservableProperty] private ObservableCollection<DiscoveredDeviceModel> discoveredDevices;

    [ObservableProperty] private string deviceNickName = string.Empty;

    #endregion
}