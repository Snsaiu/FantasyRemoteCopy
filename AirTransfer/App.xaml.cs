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

#if WINDOWS
            var window = Current.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window;

            var clip = Handler.MauiContext.Services.GetRequiredService<IClipboardWatchable>();
            (clip as ClipboardWatcher)._window = window;
            clip.Initialize(window);
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