using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Platforms;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.ViewModels;
using CommunityToolkit.Maui;
using FantasyRemoteCopy.UI.Views.Dialogs;

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
			}).UseMauiCommunityToolkit();



        builder.Services.AddTransient<IOpenFolder, DefaultOpenFolder>();
		builder.Services.AddTransient<IFileSaveLocation, AppDataFolderFileSaveLocation>();
		builder.Services.AddTransient<IGetLocalIp, DefaultLocalIp>();
		builder.Services.AddTransient<IScanLocalNetIp, DefaultScanLocalNetIp>();
		builder.Services.AddTransient<IGlobalScanLocalNetIp, GlobalScanLocalNetIp>();

		builder.Services.AddSingleton<IReceiveData, TcpReceiveData>();
		builder.Services.AddTransient<ISendData, TcpDataSender>();
		builder.Services.AddTransient<IUserService, ConfigUserService>();
		builder.Services.AddTransient<ISaveDataService,DbSaveDataService>();

		builder.Services.AddTransient<SendDataBussiness>();
		builder.Services.AddSingleton<ReceiveBussiness>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<LoginPageModel>();
		
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<HomePageModel>();

		builder.Services.AddTransient<SettingPage>();
		builder.Services.AddTransient<SettingPageModel>();

		builder.Services.AddTransient<SendTypeDialog>();

		builder.Services.AddTransient<TextInputPage>();

		builder.Services.AddTransient<ListPage>();
		builder.Services.AddTransient<ListPageModel>();

	

		return builder.Build();
	}
}

