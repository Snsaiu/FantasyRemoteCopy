using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.Configs;
using AirTransfer.Interfaces.Impls.TcpTransfer;
using AirTransfer.Interfaces.Impls.UdpTransfer;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Maui.LifecycleEvents;


namespace AirTransfer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

            builder.ConfigureLifecycleEvents(lifecycle =>
            {
#if WINDOWS
                lifecycle.AddWindows(windows => windows.OnWindowCreated((del) =>
                {
                    del.ExtendsContentIntoTitleBar = true;
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(del);
                    WindowExtensions.Hwnd = hwnd;
                }));
#endif
            });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddFluentUIComponents(options => { options.UseTooltipServiceProvider = true; });
            builder.Services.AddLocalization();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IUserService, ConfigUserService>();
            builder.Services.AddSingleton<IStateManager, StateManager>();


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

            builder.Services.AddSingleton<ISaveDataService, DbSaveDataService>();
            builder.Services.AddSingleton<ILanguageService, ConfigLanguageService>();
            builder.Services.AddSingleton<ISendPortService, SendPortService>();
            builder.Services.AddSingleton<IPortCheckable, PortChecker>();

            builder.Services.AddSingleton<IThemeService, ThemeService>();
            builder.Services.AddSingleton<IClipboardWatchable, ClipboardWatcher>();
            builder.Services.AddSingleton<ILoopWatchClipboardService, LoopWatchClipboardService>();

#if MACCATALYST || WINDOWS
            builder.Services.AddSingleton<ITrayService, TrayService>();
#endif

            return builder.Build();
        }
    }
}