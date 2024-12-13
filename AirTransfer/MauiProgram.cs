using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.Configs;

using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;


namespace AirTransfer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });


            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents(options => { options.UseTooltipServiceProvider = true; });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IUserService, ConfigUserService>();
            builder.Services.AddSingleton<IStateManager, StateManager>();


            return builder.Build();
        }
    }
}