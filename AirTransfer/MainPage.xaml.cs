using AirTransfer.Interfaces;

namespace AirTransfer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetupTrayIcon();
        }

        private void SetupTrayIcon()
        {
#if MACCATALYST || WINDOWS
            var trayService = Extensions.ServiceProvider.RequestService<ITrayService>();
            if (trayService != null)
            {
                trayService.Initialize();
                trayService.ClickHandler = () => { };
            }
#endif
        }
    }
}