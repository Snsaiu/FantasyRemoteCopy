using CommunityToolkit.Maui;

using FantasyMvvm;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Platforms.Windows;
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
        builder.Services.AddSingleton<LocalIpScannerBase, DefaultScanLocalNetIp>();
        builder.Services.AddSingleton<LocalNetJoinRequestBase, LocalNetJoinRequest>();

        builder.Services.AddSingleton<DeviceLocalIpBase, DefaultLocalIp>();

        builder.Services.AddSingleton<ISystemType, SystemTypeProvider>();
        builder.Services.AddSingleton<IDeviceType, DeviceTypeProvider>();

        builder.Services.AddSingleton<LocalNetJoinProcessBase, LocalNetJoinProcess>();
        builder.Services.AddSingleton<GlobalScanBase, GlobalScan>();

        builder.Services.AddSingleton<TcpSendFileBase, TcpSendFile>();
        builder.Services.AddSingleton<TcpSendTextBase, TcpSendText>();

        builder.Services.AddSingleton<TcpLoopListenContentBase, TcpLoopListenContent>();

        // builder.Services.AddSingleton<IGetLocalNetDevices, DefaultScanLocalNetIp>();

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