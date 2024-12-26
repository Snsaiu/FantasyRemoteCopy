using System.Globalization;
using System.Reflection.Metadata;

using AirTransfer.Interfaces;
using AirTransfer.Language;
using AirTransfer.Resources.Languages;

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
            InitLanguage();

#if WINDOWS ||MACCATALYST
            var window = new Window(new MainPage()) { Title = "AirTransfer", Width = 600, MinimumWidth = 600 };

            return window;
#endif

            return new Window(new MainPage()) { Title = "AirTransfer" };
        }

        protected override void OnStart()
        {
            base.OnStart();

        SetupTrayIcon();
        }

        private void SetupTrayIcon()
        {

#if MACCATALYST
            
            
            var trayService = Handler.MauiContext.Services.GetRequiredService<ITrayService>();
           
            if (trayService != null)
            {
                trayService.Initialize();
                trayService.ClickHandler = () => { };
            }
#endif
         
        }

        private void InitLanguage()
        {
            var languageService = Handler.MauiContext.Services.GetRequiredService<ILanguageService>();
            var language = languageService.GetLanguage();
            LocalizationResourceManager.Instance.SetCulture(new(language));
        }
    }
}