//---
//.net MAUI how to maximize application on startup
//https://stackoverflow.com/questions/72128525/net-maui-how-to-maximize-application-on-startup
using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
//---.net MAUI how to maximize application on startup

using Microsoft.Extensions.Logging;

namespace VPOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
        //---
        //.net MAUI how to maximize application on startup
        https://stackoverflow.com/questions/72128525/net-maui-how-to-maximize-application-on-startup
#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
                    if(winuiAppWindow.Presenter is OverlappedPresenter p)
                    {
                        p.Maximize();
                    }

                    //---
                    //[Windows] How to completely hide the TitleBar? 
                    //https://github.com/dotnet/maui/issues/15142
                    //https://www.cnblogs.com/whuanle/p/17060473.html [疯狂吐槽 MAUI 以及 MAUI 入坑知识点]
                    window.ExtendsContentIntoTitleBar = false;
                    // 保留任务栏
                    switch (winuiAppWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(false, false);
                            overlappedPresenter.Maximize();
                            break;
                    }
                    // 全屏时去掉任务栏
                    winuiAppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                    //---[Windows] How to completely hide the TitleBar? 

                });
            });
        });
#endif
            //---.net MAUI how to maximize application on startup
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}