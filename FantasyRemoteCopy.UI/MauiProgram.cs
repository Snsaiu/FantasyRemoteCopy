﻿using CommunityToolkit.Maui;

using FantasyMvvm;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;
using FantasyRemoteCopy.UI.Interfaces.Impls.UdpTransfer;
using FantasyRemoteCopy.UI.ViewModels;
using FantasyRemoteCopy.UI.Views;
using FantasyRemoteCopy.UI.Views.Dialogs;

using Microsoft.Extensions.Logging;

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
            .UseFantasyApplication();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IOpenFolder, DefaultOpenFolder>();
        builder.Services.AddSingleton<IFileSavePath, FileSavePath>();

        builder.Services.AddSingleton<LocalNetDeviceDiscoveryBase, LocalNetDeviceDiscovery>();
        builder.Services.AddSingleton<LocalNetInviteDeviceBase, LocalNetInviteDevice>();
        builder.Services.AddSingleton<LocalIpScannerBase, DefaultScanLocalNetIp>();
        builder.Services.AddSingleton<LocalNetJoinRequestBase, LocalNetJoinRequest>();
        builder.Services.AddSingleton<FileSavePathBase, FileSavePath>();
        builder.Services.AddSingleton<ISavePathService, SavePathService>();


        builder.Services.AddSingleton<DeviceLocalIpBase, DefaultLocalIp>();

        builder.Services.AddSingleton<ISystemType, SystemTypeProvider>();
        builder.Services.AddSingleton<IDeviceType, DeviceTypeProvider>();

        builder.Services.AddSingleton<IOpenFileable, OpenFileProvider>();

        builder.Services.AddSingleton<LocalNetJoinProcessBase, LocalNetJoinProcess>();
        builder.Services.AddSingleton<GlobalScanBase, GlobalScan>();

        builder.Services.AddSingleton<TcpSendFileBase, TcpSendFile>();
        builder.Services.AddSingleton<TcpSendTextBase, TcpSendText>();


        builder.Services.AddSingleton<TcpLoopListenContentBase, TcpLoopListenContent>();

        // builder.Services.AddSingleton<IGetLocalNetDevices, DefaultScanLocalNetIp>();

        builder.Services.AddSingleton<IUserService, ConfigUserService>();
        builder.Services.AddSingleton<ISaveDataService, DbSaveDataService>();
        builder.Services.AddSingleton<ILanguageService, ConfigLanguageService>();
        builder.Services.AddSingleton<ISendPortService, SendPortService>();
        builder.Services.AddSingleton<IPortCheckable, PortChecker>();

        builder.UseRegisterPage<LoginPage, LoginPageModel>();
        builder.UseRegisterPage<HomePage, HomePageModel>();
        builder.UseRegisterPage<SettingPage, SettingPageModel>();
        builder.UseRegisterPage<DetailPage, DetailPageModel>();
        builder.UseRegisterPage<TextInputPage, TextInputPageModel>();

        builder.UseRegisterPage<ListPage, ListPageModel>();

        return builder.UseGetProvider().Build();
    }
}