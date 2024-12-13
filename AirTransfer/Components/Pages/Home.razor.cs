using AirTransfer.Interfaces;
using AirTransfer.Models;

using FantasyRemoteCopy.UI.Consts;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using System.ComponentModel.Design;
using AirTransfer.Enums;
using FantasyResultModel;

namespace AirTransfer.Components.Pages;

public partial class Home : ComponentBase
{


    [Parameter] public string Text { get; set; }


    protected override Task OnInitializedAsync()
    {
        if (StateManager.ExistKey(ConstParams.StateManagerKeys.ListenKey))
        {
            var state = StateManager.GetState<bool>(ConstParams.StateManagerKeys.ListenKey);
            if (state)
                InitListenAsync();
        }
        else
        {
            StateManager.SetState(ConstParams.StateManagerKeys.ListenKey, true);
            InitListenAsync();
        }
        return base.OnInitializedAsync();

    }



    #region Commands
    private void GotoTextInputPageCommand()
    {
        NavigationManager.NavigateTo("/Home/TextInput");
    }

    #endregion

    #region Private Methods

    private async Task InitListenAsync()
    {
        await Init();
        await SetReceive();
    }


    public async Task Init()
    {
        var userRes = await UserService.GetCurrentUserAsync();
        UserName = userRes.Data.Name;
        DeviceNickName = userRes.Data.DeviceNickName;
        localDevice = new DeviceModel()
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

        try
        {
            var sendfile = InformationModel!.SendType == SendType.Folder
                ? new SendCompressFileModel(localIp, targetIp, InformationModel.FolderPath, port)
                : InformationModel.SendType == SendType.File
                    ? InformationModel.Files.Count() == 1
                                    ? new SendFileModel(localIp, targetIp, InformationModel.Files.First(), port)
                                    : new SendCompressFileModel(localIp, targetIp, InformationModel.Files, port)
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
                ? DiscoveredDevices.FirstOrDefault(y => y.Flag == x.TargetFlag)
                : DiscoveredDevices.FirstOrDefault(y => y.Flag == x.Flag);
            if (flag is null)
                return;
            if (isSendModel)
            {
                Logger.LogInformation($"发送数据到{flag.Flag} 进度为{flag.Progress}");
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
                Logger.LogInformation($"接收数据{flag.Flag} 进度为{flag.Progress}");
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
        var device = DiscoveredDevices.FirstOrDefault(x => x.Flag == codeWord.Flag);
        if (device is null) return;
        device.WorkState = WorkState.Sending;
        device.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid, codeWord.TargetFlag,
            codeWord.Flag, codeWord.Port, codeWord.SendType, codeWord.CancellationTokenSource));
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

            Logger.LogInformation("发现本地ip:{0}", localDevice.Flag);

            var devices = LocalIpScannerBase.GetDevicesAsync(default);

            await foreach (var device in devices)
            {
                Logger.LogInformation("通过设备发现扫描到的ip:{0}", device.Flag);

                await LocalNetInviteDeviceBase.SendAsync(
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
        DataService.AddAsync(saveDataModel);
    }

    #endregion
}