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
        Task.Run(async () =>
        {
            await Init();
            SetReceive();
        });
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


    public async Task Init()
    {
        ResultBase<UserInfo> userRes = await userService.GetCurrentUserAsync();
        UserName = userRes.Data.Name;
        DeviceNickName = userRes.Data.DeviceNickName;
        localDevice = new DeviceModel()
        {
            DeviceName = DeviceNickName, DeviceType = _deviceType.Device.ToString(),
            Flag = await _deviceLocalIpBase.GetLocalIpAsync(),
            NickName = UserName, SystemType = _systemType.System
        };
        //InitData();
    }


    private void InitData()
    {
        DiscoveredDevices.Add(new DiscoveredDeviceModel
            { Flag = "192.168.1.1", DeviceName = "my pc", NickName = "dfdf", SystemType = SystemType.Windows });
        DiscoveredDevices.Add(new DiscoveredDeviceModel
            { Flag = "192.168.1.2", DeviceName = "my pc", NickName = "我的mac", SystemType = SystemType.MacOS });
    }


    /// <summary>
    /// 检查当前是否还有任务在运行
    /// </summary>
    /// <returns>如果有任务，返回true，否则返回false</returns>
    private bool HasTaskRunning() => DiscoveredDevices.Any() && DiscoveredDevices.Any(x => x.TransmissionTasks.Any());


    [RelayCommand]
    private async Task CloseTransformAsync(DiscoveredDeviceModel device)
    {
        if (device.TransmissionTasks.FirstOrDefault() is { } receiveTask)
        {
            var cancelCodeWord = CodeWordModel.CreateCodeWord(receiveTask.TaskGuid, CodeWordType.CancelTransmission,
                receiveTask.Flag,
                receiveTask.TargetFlag, receiveTask.Port, receiveTask.SendType, localDevice, null);

            this.logger.LogInformation(
                $"请求 {cancelCodeWord.Flag} 与 {cancelCodeWord.TargetFlag} 通过端口 {cancelCodeWord.Port} 的数据传输人工中断");

            await this._tcpSendTextBase.SendAsync(
                new SendTextModel(receiveTask.Flag, receiveTask.TargetFlag, cancelCodeWord.ToJson(),
                    ConstParams.TCP_PORT), null, default);

            receiveTask.CancellationTokenSource.Cancel();

            device.TransmissionTasks.Remove(receiveTask);
        }

        SendCommand.NotifyCanExecuteChanged();

        device.Progress = 0;
        device.WorkState = WorkState.None;
    }


    private void SenderAddTaskAndShowProgress(CodeWordModel codeWord, CancellationTokenSource cancelTokenSource)
    {
        var device = DiscoveredDevices.FirstOrDefault(x => x.Flag == codeWord.Flag);
        if (device is null) return;
        device.WorkState = WorkState.Sending;
        device.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.TargetFlag,
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

    private async Task SendFileAsync(string targetIp, int port, CancellationToken token, string taskId)
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

            await _tcpSendFileBase.SendAsync(sendfile, ReportProgress(true, taskId), token);
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
        return DiscoveredDevices.Any(x => x.IsChecked && !x.TransmissionTasks.Any()) && InformationModel is not null;
    }

    [RelayCommand(CanExecute = nameof(CanSend))]
    private async Task Send()
    {
        // 首先向目标电脑发送一个端口用于检查是否可以使用该端口
        foreach (var item in DiscoveredDevices)
        {
            if (!item.IsChecked)
                continue;

            var codeWord = CodeWordModel.CreateCodeWord(Guid.NewGuid().ToString("N"), CodeWordType.CheckingPort,
                localDevice.Flag,
                item.Flag, 5005,
                InformationModel.SendType, localDevice, null);


            var portCheckMessage = new SendTextModel(localDevice.Flag, item.Flag ?? throw new NullReferenceException(),
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

            logger.LogInformation("发现本地ip:{0}", localDevice.Flag);

            IAsyncEnumerable<ScanDevice> devices = _localIpScannerBase.GetDevicesAsync(default);

            await foreach (var device in devices)
            {
                logger.LogInformation("通过设备发现扫描到的ip:{0}", device.Flag);

                await _localNetInviteDeviceBase.SendAsync(
                    new DeviceDiscoveryMessage(UserName, localDevice.Flag, device.Flag) ??
                    throw new NullReferenceException(), default);
            }
        }
        finally
        {
            IsBusy = false;
            await Task.Yield();
        }
    }


    private IProgress<ProgressValueModel> ReportProgress(bool isSendModel, string taskId)
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
                    flag.RemoveTransmissionTask(taskId);
                }
                else
                {
                    flag.WorkState = flag.TransmissionTasks.Any() ? WorkState.Sending : WorkState.None;
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

            Application.Current.Dispatcher.Dispatch(() => { SendCommand.NotifyCanExecuteChanged(); });
            flag.Progress = x.Progress;
        });

        return progress;
    }
}