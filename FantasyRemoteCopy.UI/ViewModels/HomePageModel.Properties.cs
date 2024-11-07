using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.HttpsTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
using FantasyRemoteCopy.UI.Models;
using Microsoft.Extensions.Logging;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class HomePageModel
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