using System.Collections.Specialized;
using AirTransfer.Consts;
using AirTransfer.Interfaces;
using AirTransfer.Models;
using Microsoft.AspNetCore.Components;
using AirTransfer.Enums;
using AirTransfer.Extensions;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;

namespace AirTransfer.Components.Pages;

public partial class Home : PageComponentBase
{
    #region Override Methods

    public override Task SetParametersAsync(ParameterView parameters)
    {
        StateManager.ObservableDevices().CollectionChanged -= UpdateDevices;
        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Init();

        CheckWorkBusyState();

        if (StateManager.ExistKey(ConstParams.StateManagerKeys.ListenKey))
        {
            var state = StateManager.GetState<bool>(ConstParams.StateManagerKeys.ListenKey);
            if (!state)
            {
                StateManager.SetState(ConstParams.StateManagerKeys.ListenKey, true);
                await InitListenAsync();
            }
        }
        else
        {
            StateManager.SetState(ConstParams.StateManagerKeys.ListenKey, true);
            await InitListenAsync();

        }


        StateManager.ObservableDevices().CollectionChanged += UpdateDevices;
    }

    private void CheckWorkBusyState()
    {
        var noWork = StateManager.Devices().All(x => x.WorkState == WorkState.None);
        StateManager.SetIsWorkingBusyState(!noWork);
    }


    private void UpdateDevices(object? sender, NotifyCollectionChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    protected override Task OnPageInitializedAsync(string? url, Dictionary<string, object>? data)
    {
        if (url is null)
            return Task.CompletedTask;
        if (url == "/home/text-input" && data != null && data.ContainsKey("text"))
        {
            StateManager.SetInformationModel(new()
            {
                Text = data["text"].ToString(),
                SendType = SendType.Text
            });
        }

        return Task.CompletedTask;
    }

    #endregion


    #region Commands



    private async Task CloseTransformCommand(DiscoveredDeviceModel device)
    {
        if (device.TransmissionTasks.FirstOrDefault() is { } receiveTask)
        {
            var cancelCodeWord = CodeWordModel.CreateCodeWord(receiveTask.TaskGuid, CodeWordType.CancelTransmission,
                receiveTask.Flag,
                receiveTask.TargetFlag, receiveTask.Port, receiveTask.SendType, LocalDevice, null);

            Logger.LogInformation(
                $"请求 {cancelCodeWord.Flag} 与 {cancelCodeWord.TargetFlag} 通过端口 {cancelCodeWord.Port} 的数据传输人工中断");

            await TcpSendTextBase.SendAsync(
                new(receiveTask.Flag, receiveTask.TargetFlag, cancelCodeWord.ToJson(),
                    ConstParams.TCP_PORT), null, default);

            receiveTask.CancellationTokenSource?.Cancel();

            device.TransmissionTasks.Remove(receiveTask);
        }

        device.Progress = 0;
        device.WorkState = WorkState.None;
        CheckWorkBusyState();
    }


    private Task SearchCommand()
    {
        return DeviceDiscoverAsync();
    }

    private Task SendCommand()
    {
        return SendAsync();
    }

    private async Task SendAsync()
    {
        StateManager.SetIsWorkingBusyState(true);
        // 首先向目标电脑发送一个端口用于检查是否可以使用该端口
        foreach (var item in StateManager.Devices().Where(x => x.IsChecked))
        {
            var information = CloneHelper.DeepClone(StateManager.GetInformationModel());
            if (information is null)
                throw new NullReferenceException();

            var codeWord = CodeWordModel.CreateCodeWord(Guid.NewGuid().ToString("N"), CodeWordType.CheckingPort,
                LocalDevice.Flag ?? throw new NullReferenceException(),
                item.Flag ?? throw new NullReferenceException(), 5005,
                information.SendType, LocalDevice, null);


            var portCheckMessage = new SendTextModel(LocalDevice.Flag, item.Flag ?? throw new NullReferenceException(),
                codeWord.ToJson(), ConstParams.TCP_PORT);
            await TcpSendTextBase.SendAsync(portCheckMessage, null, default);
        }
    }

    private void ClearInformationModelCommand()
    {
        StateManager.SetInformationModel(null);
    }

    private void GotoTextInputPageCommand()
    {
        NavigationManager.NavigateTo("/home/text-input");
    }

    private async Task OpenFileCommand()
    {
        var files = await FilePicker.PickMultipleAsync();

        if (!files.Any())
            return;
        StateManager.SetInformationModel(new()
        {
            SendType = SendType.File,
            Files = files.Select(x => x.FullPath)
        });
    }

    private async Task OpenFolderCommand()
    {
        var folerPicker = await FolderPicker.PickAsync(default);
        if (!folerPicker.IsSuccessful)
            return;

        StateManager.SetInformationModel(new()
        {
            SendType = SendType.Folder,
            FolderPath = folerPicker.Folder.Path
        });
    }

    #endregion

    #region Private Methods

    private async void ReceiveClipboard(object data)
    {
        if (!StateManager.ExistKey(ConstParams.StateManagerKeys.LoopWatchClipboardKey))
        {
            var state = LoopWatchClipboardService.GetState();
            StateManager.SetState(ConstParams.StateManagerKeys.LoopWatchClipboardKey, state);
            if (!state)
                return;
        }

        if (!StateManager.GetState<bool>(ConstParams.StateManagerKeys.LoopWatchClipboardKey) || !StateManager.Devices().Any(x => x.IsChecked))
            return;


        Application.Current.Dispatcher.Dispatch(() =>
        {
            // var informationBackup = CloneHelper.DeepClone(StateManager.GetInformationModel());
            StateManager.SetInformationModel((new InformationModel() { SendType = SendType.Text, Text = data.ToString() }));
            SendAsync();
        });


    }

    // private void InitData()
    // {
    //     DiscoveredDevices.Add(new DiscoveredDeviceModel
    //     {
    //         Flag = "192.168.1.1",
    //         DeviceName = "my window",
    //         NickName = "�ҵ�windows",
    //         SystemType = Enums.SystemType.Windows
    //     });
    //     DiscoveredDevices.Add(new DiscoveredDeviceModel
    //     {
    //         Flag = "192.168.1.2",
    //         DeviceName = "my macos",
    //         NickName = "�ҵ�mac",
    //         SystemType = Enums.SystemType.MacOS
    //     });
    //     DiscoveredDevices.Add(new DiscoveredDeviceModel
    //         { Flag = "192.168.1.2", DeviceName = "my ios", NickName = "�ҵ�iphone", SystemType = Enums.SystemType.IOS });
    //     DiscoveredDevices.Add(new DiscoveredDeviceModel
    //     {
    //         Flag = "192.168.1.2",
    //         DeviceName = "my android",
    //         NickName = "�ҵ�android",
    //         SystemType = Enums.SystemType.Android
    //     });
    // }


    private async Task InitListenAsync()
    {
        await SetReceive();

#if WINDOWS || MACCATALYST
        ClipboardWatchable.Initialize(null);
        ClipboardWatchable.ClipboardUpdate += ReceiveClipboard;
#endif

    }


    private async Task Init()
    {
        var userRes = await UserService.GetCurrentUserAsync();
        UserName = userRes.Data.Name;
        DeviceNickName = userRes.Data.DeviceNickName;
        LocalDevice = new DeviceModel()
        {
            DeviceName = DeviceNickName,
            DeviceType = DeviceType.Device.ToString(),
            Flag = await DeviceLocalIpBase.GetLocalIpAsync(),
            NickName = UserName,
            SystemType = SystemType.System
        };
    }

    private async Task SendFileAsync(string targetIp, int port, CancellationToken token, string taskId)
    {
        var localIp = await DeviceLocalIpBase.GetLocalIpAsync();

        var information = StateManager.GetInformationModel();
        if (information is null)
            throw new NullReferenceException();

        try
        {
            var sendfile = information.SendType == SendType.Folder
                ? new SendCompressFileModel(localIp, targetIp,
                    information.FolderPath ?? throw new NullReferenceException(), port)
                : information.SendType == SendType.File
                    ? information.Files.Count() == 1
                        ? new SendFileModel(localIp, targetIp, information.Files.First(), port)
                        : new SendCompressFileModel(localIp, targetIp, information.Files, port)
                    : throw new NotSupportedException();
            await TcpSendFileBase.SendAsync(sendfile, ReportProgress(true, taskId), token);
        }
        catch (OperationCanceledException)
        {
            //  device.WorkState = WorkState.None;
        }
    }

    private IProgress<ProgressValueModel>
        ReportProgress(bool isSendModel, string taskId)
    {
        Progress<ProgressValueModel> progress = new(x =>
        {
            var flag = isSendModel
                ? StateManager.FindDiscoveredDeviceModel(x.TargetFlag)
                : StateManager.FindDiscoveredDeviceModel(x.Flag);
            if (flag is null)
                return;
            if (isSendModel)
            {
                Logger.LogInformation($"发送数据到{flag.Flag}， 进度{flag.Progress}");
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
                Logger.LogInformation($"接收{flag.Flag}，进度Ϊ{flag.Progress}");
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
            var noWork = StateManager.Devices().All(x => x.WorkState == WorkState.None);
            if (StateManager.ExistKey(ConstParams.StateManagerKeys.CurrentUriKey))
            {
                var value = StateManager.GetState<string>(ConstParams.StateManagerKeys.CurrentUriKey);
                if (value == "home")
                    StateManager.SetIsWorkingBusyState(!noWork);
            }

            if (noWork)
            {
                StateManager.SetInformationModel(null);
            }
        });

        return progress;
    }

    private void SenderAddTaskAndShowProgress(CodeWordModel codeWord, CancellationTokenSource cancelTokenSource)
    {
        var device = StateManager.FindDiscoveredDeviceModel(codeWord.Flag);
        if (device is null) return;
        device.WorkState = WorkState.Sending;
        device.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.TargetFlag,
            codeWord.Flag, codeWord.Port, codeWord.SendType, cancelTokenSource));
    }

    private async Task DeviceDiscoverAsync()
    {
        try
        {
            IsBusy = true;

            StateManager.ClearDiscoveryModel();
            await InvokeAsync(StateHasChanged);

            var devices = LocalIpScannerBase.GetDevicesAsync(default);

            await foreach (var device in devices)
            {
                try
                {
                    await LocalNetInviteDeviceBase.SendAsync(
                        new(UserName, LocalDevice.Flag, device.Flag), default);
                }
                catch (Exception exception)
                {
                    Logger.LogError(exception, "设备发现出错");
                }
            }
        }
        finally
        {
            IsBusy = false;
        }
    }


    private Task SaveDataToLocalDbAsync(TransformResultModel<string> data)
    {
        var saveDataModel = new SaveDataModel
        {
            DataType = data.SendType,
            Content = data.Result,
            Time = DateTime.Now
        };

        var model = StateManager.FindDiscoveredDeviceModel(data.Flag);
        if (model is null)
            throw new NullReferenceException();
        model.WorkState = WorkState.None;
        NewMessageVisible = true;
        IsDownLoadingVisible = false;
        saveDataModel.SourceDeviceNickName = model.NickName ?? string.Empty;
        saveDataModel.Guid = Guid.NewGuid().ToString();
        DataService.AddAsync(saveDataModel);

        var noWork = StateManager.Devices().All(x => x.WorkState == WorkState.None);
        StateManager.SetIsWorkingBusyState(!noWork);

        InvokeAsync(() =>
        {
            StateManager.AppendNotReadCount();
            StateHasChanged();
        });

        return Task.CompletedTask;
    }

    #endregion
}