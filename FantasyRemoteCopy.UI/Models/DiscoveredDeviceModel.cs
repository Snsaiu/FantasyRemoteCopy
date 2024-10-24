using CommunityToolkit.Mvvm.ComponentModel;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI.Models
{

    public enum WorkState
    {
        None,
        Sending,
        Downloading,
    }

    /// <summary>
    /// 设备发现的模型
    /// </summary>
    public partial class DiscoveredDeviceModel : ObservableObject, IFlag
    {

        [ObservableProperty]
        private string? deviceType;

        [ObservableProperty]
        private string? nickName;

        [ObservableProperty]
        private string? flag;

        [ObservableProperty]
        private string? deviceName;
        
        [ObservableProperty]
        private SystemType systemType;


        [ObservableProperty]
        private WorkState workState;

        [ObservableProperty]
        private double progress;

        [ObservableProperty]
        private bool isChecked;



        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();


        public static implicit operator DiscoveredDeviceModel(JoinMessageModel model)
        {
          
            return new DiscoveredDeviceModel()
            {
                NickName = model.DeviceName,
                Flag = model.Flag,
                SystemType = model.SystemType,
                DeviceType = model.Device.ToString()
            };
        }

    }
}
