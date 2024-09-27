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
        private ImageSource? img;

        [ObservableProperty]
        private string? deviceName;


        [ObservableProperty]
        private WorkState workState;

        [ObservableProperty]
        private double progress;


        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();


        public static implicit operator DiscoveredDeviceModel(JoinMessageModel model)
        {
            ImageSource? img = model.SystemType switch
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
                Img = img,
                DeviceType = model.Device.ToString()
            };
        }

    }
}
