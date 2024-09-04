using CommunityToolkit.Maui;

using FantasyMvvm;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Platforms;
using FantasyRemoteCopy.UI.ViewModels;
using FantasyRemoteCopy.UI.ViewModels.DialogModels;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

namespace FantasyRemoteCopy.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
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
        
        builder.Services.AddSingleton<IOpenFolder, DefaultOpenFolder>();
		builder.Services.AddSingleton<IFileSaveLocation, AppDataFolderFileSaveLocation>();

        builder.Services.AddSingleton<IGetLocalIp, DefaultLocalIp>();
        builder.Services.AddSingleton<IScanLocalNetIp, DefaultScanLocalNetIp>();
        builder.Services.AddSingleton<IGlobalScanLocalNetIp, GlobalScanLocalNetIp>();

        builder.Services.AddSingleton<IReceiveData, TcpReceiveData>();
        builder.Services.AddSingleton<ISendData, TcpDataSender>();
        builder.Services.AddSingleton<IUserService, ConfigUserService>();
        builder.Services.AddSingleton<ISaveDataService, DbSaveDataService>();

        builder.Services.AddSingleton<SendDataBussiness>();
        builder.Services.AddSingleton<ReceiveBussiness>();

        builder.UseRegisterPage<LoginPage, LoginPageModel>();
        builder.UseRegisterPage<HomePage, HomePageModel>();
        builder.UseRegisterPage<SettingPage, SettingPageModel>();

        builder.UseRegisterPage<TextInputPage, TextInputPageModel>();

        builder.UseRegisterPage<ListPage, ListPageModel>();
        builder.UseRegisterDialog<SendTypeDialog, SendTypeDialogModel>();

        return builder.Build();
    }
}

