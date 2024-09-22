using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models
{
    /// <summary>
    /// 设备发现的模型
    /// </summary>
    public partial class DiscoveredDeviceModel : ObservableObject,IFlag
    {

        [ObservableProperty]
        private string deviceType;

        [ObservableProperty]
        private string nickName;

        [ObservableProperty]
        private string flag;

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
        
        
        public static implicit operator DiscoveredDeviceModel(JoinMessageModel model)
        {
            return new DiscoveredDeviceModel()
            {
                NickName = model.DeviceName,
                Flag = model.Flag
            };
        }

    }
}
