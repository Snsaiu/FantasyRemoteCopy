using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Platforms;
using Microsoft.Extensions.DependencyInjection;

namespace FantasyRemoteCopy.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddTransient<IGetLocalIp, DefaultLocalIp>();
		builder.Services.AddTransient<IScanLocalNetIp, DefaultScanLocalNetIp>();
		builder.Services.AddTransient<MainPage>();
		return builder.Build();
	}
}

