using AirTransfer.Components.Components;
using AirTransfer.Enums;
using AirTransfer.Interfaces.IConfigs;
using AirTransfer.Language;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI.Windowing;
#endif


namespace AirTransfer.Extensions;

public static class MauiAppExtension
{
    public static void ConfigureLifecycle(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(lifecycle =>
        {
#if WINDOWS
            lifecycle.AddWindows(windows => windows.OnWindowCreated((del) =>
            {
                del.ExtendsContentIntoTitleBar = true;
                // del.SetTitleBar(null);

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(del);
                WindowExtensions.Hwnd = hwnd;

                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                appWindow.SetIcon(null);

                appWindow.Closing += async (sender, args) =>
                {
                    var dialogService = ServiceProvider.RequestService<IDialogService>();
                    args.Cancel = true;
                    //get all of Microsoft.Maui.Controls.windows.
                    var windows1 = Application.Current.Windows.ToList<Window>();
                    var title = LocalizationResourceManager.Instance["TabbyCat"];
                    foreach (var win in windows1)
                    {
                        if (win.Title == title)
                        {
                            var showCloseDialogService = ServiceProvider.RequestService<IShowCloseDialogService>();
                            var closeAppBehaviorService = ServiceProvider.RequestService<ICloseAppBehaviorService>();

                            if (showCloseDialogService.Get<bool>())
                            {
                                // 不在显示

                                var closeState = (CloseAppBehavior)closeAppBehaviorService.Get<int>();

                                if (closeState == CloseAppBehavior.Exit)
                                {
                                    Application.Current.CloseWindow(win);
                                }
                                else
                                {
                                    var p = appWindow.Presenter as OverlappedPresenter;
                                    p?.Minimize();
                                    return;
                                }
                            }

                            var dialogResult = await dialogService.ShowDialogAsync<CloseDialog>(new()
                            {
                                ShowDismiss = false,
                                Title = LocalizationResourceManager.Instance["Warning"],
                                PrimaryAction = LocalizationResourceManager.Instance["MinimizeToTray"],
                                SecondaryAction = LocalizationResourceManager.Instance["ExitTheApplication"]
                            });
                            var result = await dialogResult.Result;
                            if (!result.Cancelled)
                            {
                                if (showCloseDialogService.Get<bool>())
                                    closeAppBehaviorService.Set<int>((int)CloseAppBehavior.Minimize);
                                var p = appWindow.Presenter as OverlappedPresenter;
                                p?.Minimize();
                                await dialogResult.CloseAsync();

                            }

                            else
                            {
                                if (showCloseDialogService.Get<bool>())
                                    closeAppBehaviorService.Set<int>((int)CloseAppBehavior.Exit);
                                Application.Current.CloseWindow(win);
                            }
                        }
                    }
                };
            }));
#endif
            
#if MACCATALYST
            // lifecycle.AddiOS(iOS => iOS.WillTerminate(application =>
            // {
            //     // 在应用即将关闭时执行最小化逻辑
            //     UIApplication.SharedApplication.KeyWindow?.ResignKeyWindow();
            // }));
            //
            //
            //    lifecycle.AddiOS(iOS=> iOS.OnActivated(window =>
            //     {
            //         MacOSWindowExtensions.EnableCloseButtonAsMinimize();
            //     }));
           
 #endif
        });
    }
}
