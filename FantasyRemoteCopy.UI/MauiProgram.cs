using CommunityToolkit.Maui;

using FantasyMvvm;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
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
            .UseFantasyApplication();


        builder.Services.AddSingleton<IOpenFolder, DefaultOpenFolder>();
        builder.Services.AddSingleton<IFileSaveLocation, AppDataFolderFileSaveLocation>();

        builder.Services.AddSingleton<LocalNetDeviceDiscoveryBase, LocalNetDeviceDiscovery>();
        builder.Services.AddSingleton<LocalNetInviteDeviceBase, LocalNetInviteDevice>();
        

        builder.Services.AddSingleton<IGetLocalIp, DefaultLocalIp>();
        builder.Services.AddSingleton<IGetLocalNetDevices, DefaultScanLocalNetIp>();

        builder.Services.AddSingleton<IReceiveData, TcpReceiveData>();
        builder.Services.AddSingleton<ISendData, TcpDataSender>();
        builder.Services.AddSingleton<IUserService, ConfigUserService>();
        builder.Services.AddSingleton<ISaveDataService, DbSaveDataService>();

        builder.UseRegisterPage<LoginPage, LoginPageModel>();
        builder.UseRegisterPage<HomePage, HomePageModel>();
        builder.UseRegisterPage<SettingPage, SettingPageModel>();
        builder.UseRegisterPage<DetailPage, DetailPageModel>();
        builder.UseRegisterPage<TextInputPage, TextInputPageModel>();

        builder.UseRegisterPage<ListPage, ListPageModel>();
        builder.UseRegisterDialog<SendTypeDialog, SendTypeDialogModel>();

        return builder.UseGetProvider().Build();
    }
}

