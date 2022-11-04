using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyRemoteCopy.UI.Models
{
    /// <summary>
    /// 设备发现的模型
    /// </summary>
    [ObservableObject]
    public partial class DiscoveredDeviceModel
    {

        [ObservableProperty]
        private string deviceType;

        [ObservableProperty]
        private string nickName;

        [ObservableProperty]
        private string ip;

        [ObservableProperty]
        private ImageSource img;

        [ObservableProperty]
        private string deviceName;

        [ObservableProperty]
        private bool isDownLoading = false;

        [ObservableProperty]
        private bool isSendingData = false;

        [ObservableProperty]
        private double downloadProcess = 0;

    }
}
