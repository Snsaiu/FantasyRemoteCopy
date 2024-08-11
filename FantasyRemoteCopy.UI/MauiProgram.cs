using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.ViewModels;
using CommunityToolkit.Maui;
using FantasyRemoteCopy.UI.Views.Dialogs;
using FantasyMvvm;
using FantasyRemoteCopy.UI.ViewModels.DialogModels;
using FantasyRemoteCopy.Core.Platforms;

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

		builder.UseRegisterPage<LoginPage, LoginPageModel>();
		builder.UseRegisterPage<HomePage,HomePageModel>();
		builder.UseRegisterPage<SettingPage,SettingPageModel>();


		builder.UseRegisterPage<TextInputPage,TextInputPageModel>();

		builder.UseRegisterPage<ListPage, ListPageModel>();
		builder.UseRegisterDialog<SendTypeDialog, SendTypeDialogModel>();



	

		return builder.Build();
	}
}

