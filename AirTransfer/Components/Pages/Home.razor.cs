using AirTransfer.Interfaces;
using AirTransfer.Models;
using FantasyRemoteCopy.UI.Consts;
using Microsoft.AspNetCore.Components;
using AirTransfer.Enums;
using AirTransfer.Extensions;
using CommunityToolkit.Maui.Storage;

namespace AirTransfer.Components.Pages;

public partial class Home : PageComponentBase
{
    #region Override Methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await Init();
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

        //  InitData();
    }

    protected override Task OnPageInitializedAsync(string? url, Dictionary<string, object>? data)
    {
        if (url is null)
            return Task.CompletedTask;
        if (url == "/Home/TextInput" && data != null && data.ContainsKey("text"))
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

    private Task SearchCommand()
    {
        return DeviceDiscoverAsync();
    }

    private async Task SendCommand()
    {
        // 首先向目标电脑发送一个端口用于检查是否可以使用该端口
        foreach (var item in StateManager.Devices())
        {
            if (!item.IsChecked)
                continue;

            var information = StateManager.GetInformationModel();
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
        NavigationManager.NavigateTo("/Home/TextInput");
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

    private IProgress<ProgressValueModel> ReportProgress(bool isSendModel, string taskId)
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
                // Logger.LogInformation($"�������ݵ�{flag.Flag} ����Ϊ{flag.Progress}");
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
                // Logger.LogInformation($"��������{flag.Flag} ����Ϊ{flag.Progress}");
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

            //Application.Current.Dispatcher.Dispatch(() => { SendCommand.NotifyCanExecuteChanged(); });
            flag.Progress = x.Progress;
        });

        return progress;
    }

    private void SenderAddTaskAndShowProgress(CodeWordModel codeWord, CancellationTokenSource cancelTokenSource)
    {
        var device = StateManager.FindDiscoveredDeviceModel(codeWord.Flag);
        if (device is null) return;
        device.WorkState = WorkState.Sending;
        device.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.TargetFlag,
            codeWord.Flag, codeWord.Port, codeWord.SendType, codeWord.CancellationTokenSource));
    }

    /// <summary>
    ///     �豸����
    /// </summary>
    private async Task DeviceDiscoverAsync()
    {
        try
        {
            IsBusy = true;

            StateManager.ClearDiscoveryModel();

            // Logger.LogInformation("���ֱ���ip:{0}", localDevice.Flag);

            var devices = LocalIpScannerBase.GetDevicesAsync(default);

            await foreach (var device in devices)
            {
                // Logger.LogInformation("ͨ���豸����ɨ�赽��ip:{0}", device.Flag);

                await LocalNetInviteDeviceBase.SendAsync(
                    new DeviceDiscoveryMessage(UserName, LocalDevice.Flag, device.Flag) ??
                    throw new NullReferenceException(), default);
            }
        }
        finally
        {
            IsBusy = false;
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

        var model = StateManager.FindDiscoveredDeviceModel(data.Flag);
        if (model is null)
            throw new NullReferenceException();
        model.WorkState = WorkState.None;
        NewMessageVisible = true;
        IsDownLoadingVisible = false;
        saveDataModel.SourceDeviceNickName = model.NickName ?? string.Empty;
        saveDataModel.Guid = Guid.NewGuid().ToString();
        DataService.AddAsync(saveDataModel);
    }

    #endregion
}