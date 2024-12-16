namespace AirTransfer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
#if WINDOWS ||MACCATALYST
            return new Window(new MainPage()) { Title = "AirTransfer", Width = 600, MaximumWidth = 600, MinimumWidth = 500 };
#endif

            return new Window(new MainPage()) { Title = "AirTransfer" };
        }
    }
}