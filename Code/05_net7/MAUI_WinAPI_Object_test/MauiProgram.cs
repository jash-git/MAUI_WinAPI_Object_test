//---
//.net MAUI how to maximize application on startup
//https://stackoverflow.com/questions/72128525/net-maui-how-to-maximize-application-on-startup
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif
//---.net MAUI how to maximize application on startup

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace MAUI_WinAPI_Object_test
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
                    window.ExtendsContentIntoTitleBar = false;
                    switch (winuiAppWindow.Presenter)
                    {
                        case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                            overlappedPresenter.SetBorderAndTitleBar(false, false);
                            overlappedPresenter.Maximize();
                            break;
                    }
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