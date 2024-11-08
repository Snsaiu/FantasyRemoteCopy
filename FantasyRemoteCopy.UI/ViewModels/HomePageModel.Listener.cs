using System.Net;
using FantasyRemoteCopy.UI.Consts;
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
}