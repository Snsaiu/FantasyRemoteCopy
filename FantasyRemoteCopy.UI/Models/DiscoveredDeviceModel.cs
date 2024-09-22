﻿using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using Image = Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific.Image;

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
            var img = model.SystemType switch
            {
                SystemType.None => null,
                SystemType.Windows => ImageSource.FromFile("windows.png"),
                SystemType.MacOS => ImageSource.FromFile("mac.png"),
                SystemType.IOS => null,
                SystemType.Android => null,
                SystemType.Linux => null,
                _ => throw new ArgumentOutOfRangeException()
            };
            return new DiscoveredDeviceModel()
            {
                NickName = model.DeviceName,
                Flag = model.Flag,
                img = img,
                deviceType = model.Device.ToString()
            };
        }

    }
}
