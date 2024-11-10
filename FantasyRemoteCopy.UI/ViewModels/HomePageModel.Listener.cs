using System.Net;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Extensions;
using FantasyRemoteCopy.UI.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class HomePageModel
{
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
                    ReportProgress(false, string.Empty),
                    _cancelDownloadTokenSource.Token);
            })
            { IsBackground = true };
        thread.Start();
    }


    /// <summary>
    /// 根据<see cref="DeviceModel"/>检查当前列表是否包含flag，如果不存在则在列表中加入
    /// </summary>
    private void CheckDeviceExistAndSave(DeviceModel deviceModel)
    {
        if (DiscoveredDevices.Any(x => x.Flag == deviceModel.Flag))
            return;
        DiscoveredDevices.Add((DiscoveredDeviceModel)deviceModel);
    }


    private async void CheckPortEnable(TransformResultModel<string> data)
    {
        var codeWord = data.Result.ToObject<CodeWordModel>();
        if (codeWord.Type == CodeWordType.CheckingPort)
        {
            CheckDeviceExistAndSave(codeWord.DeviceModel);

            var port = codeWord.Port;
            logger.LogInformation($"端口{port}是否可用");
            var checkResult = await _portCheckable.IsPortInUse(port);
            logger.LogInformation($"端口{port}可用状态为 {checkResult}");
            var sendModel = new SendTextModel(localDevice.Flag, data.Flag,
                !checkResult
                    ? CodeWordModel.CreateCodeWord(codeWord.TaskGuid, CodeWordType.CheckingPortCanUse, localDevice.Flag,
                            data.Flag, port,
                            codeWord.SendType, localDevice, null)
                        .ToJson()
                    : CodeWordModel.CreateCodeWord(codeWord.TaskGuid, CodeWordType.CheckingPortCanNotUse,
                        localDevice.Flag,
                        data.Flag,
                        port,
                        codeWord.SendType, localDevice, null).ToJson(),
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
                    receiveDevice.TransmissionTasks.Add(new TransmissionTaskModel(codeWord.TaskGuid,
                        localDevice.Flag, codeWord.Flag, codeWord.Port, codeWord.SendType, cancelTokenSource));

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

                    SendCommand.NotifyCanExecuteChanged();
                }, IPAddress.Parse(data.Flag), port, ReportProgress(false, codeWord.TaskGuid), cancelTokenSource.Token);
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
                    device.TransmissionTasks.Remove(task);
                    device.Progress = 0;
                    device.WorkState = WorkState.None;
                    break;
                }
            }

            SendCommand.NotifyCanExecuteChanged();
        }
        else
        {
            logger.LogInformation($"发送方接收回调方{data.Flag}端口可用情况，接收方端口可用情况为 {codeWord.Type}");

            //端口不可用，进行累加
            if (codeWord.Type == CodeWordType.CheckingPortCanNotUse)
            {
                var port = codeWord.Port + 1;
                logger.LogInformation($"接收方{data.Flag}对于{codeWord.Port} 端口无法使用，所以向接收方再次发送{port}端口是否可用");
                var portCheckMessage = new SendTextModel(localDevice.Flag,
                    data.Flag ?? throw new NullReferenceException(),
                    CodeWordModel.CreateCodeWord(codeWord.TaskGuid, CodeWordType.CheckingPort, localDevice.Flag,
                            data.Flag, port,
                            codeWord.SendType, localDevice, null)
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
                            new SendTextModel(localDevice.Flag, data.Flag, InformationModel.Text, codeWord.Port), null,
                            sendCancelTokenSource.Token);
                        break;
                    case SendType.File:
                    case SendType.Folder:
                        SendFileAsync(data.Flag, codeWord.Port, sendCancelTokenSource.Token, codeWord.TaskGuid);
                        break;
                }
            }
        }
    }
}