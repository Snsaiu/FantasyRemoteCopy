using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Platforms;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.ViewModels;
using CommunityToolkit.Maui;
using FantasyRemoteCopy.UI.Views.Dialogs;
using FantasyMvvm;

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
			})
			.UseMauiCommunityToolkit()
			.UseFantasyApplication()
			.UseGetProvider();
		
        builder.Services.AddTransient<IOpenFolder, DefaultOpenFolder>();
		builder.Services.AddTransient<IFileSaveLocation, AppDataFolderFileSaveLocation>();
		builder.Services.AddTransient<IGetLocalIp, DefaultLocalIp>();
		builder.Services.AddTransient<IScanLocalNetIp, DefaultScanLocalNetIp>();
		builder.Services.AddTransient<IGlobalScanLocalNetIp, GlobalScanLocalNetIp>();

		builder.Services.AddSingleton<IReceiveData, TcpReceiveData>();
		builder.Services.AddSingleton<ISendData, TcpDataSender>();
		builder.Services.AddTransient<IUserService, ConfigUserService>();
		builder.Services.AddTransient<ISaveDataService,DbSaveDataService>();

		builder.Services.AddTransient<SendDataBussiness>();
		builder.Services.AddSingleton<ReceiveBussiness>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<LoginPageModel>();
		
		builder.UseRegistPage<HomePage,HomePageModel>("HomePage");
		builder.UseRegistPage<SettingPage,SettingPageModel>("SettingPage");

		builder.Services.AddTransient<SendTypeDialog>();

		builder.UseRegistPage<TextInputPage,TextInputPage>("TextInputPage");

		builder.UseRegistPage<ListPage, ListPageModel>("ListPage");


	

		return builder.Build();
	}
}

