﻿using System.Globalization;
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

            var title = LocalizationResourceManager.Instance["TabbyCat"];
#if WINDOWS ||MACCATALYST
            var window = new Window(new MainPage()) { Title = title, Width = 600, MinimumWidth = 600 };

            return window;
#endif

            return new(new MainPage()) { Title = title };
        }

        protected override void OnStart()
        {
            base.OnStart();
        }



        private void InitLanguage()
        {
            var languageService = Handler.MauiContext.Services.GetRequiredService<ILanguageService>();
            var language = languageService.GetLanguage();
            LocalizationResourceManager.Instance.SetCulture(new(language));
        }
    }
}