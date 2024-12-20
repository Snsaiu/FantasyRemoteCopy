using System.Globalization;
using System.Reflection.Metadata;
using AirTransfer.Interfaces;
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
            return new Window(new MainPage()) { Title = "AirTransfer", Width = 600,  MinimumWidth = 600 };
#endif

            return new Window(new MainPage()) { Title = "AirTransfer" };
        }

        private void InitLanguage()
        {
            var languageService = Handler.MauiContext.Services.GetRequiredService<ILanguageService>();
            var language = languageService.GetLanguage();
            if (string.IsNullOrEmpty(language))
                AppResources.Culture = new("zh-hans");
            else
                AppResources.Culture = new(language);
            // LocalizationResourceManager.Instance.SetCulture(new CultureInfo(language));
        }
    }
}