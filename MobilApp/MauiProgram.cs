﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MobilApp_Szakdolgozat.Platforms.Android;

namespace MobilApp_Szakdolgozat
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(h =>
                {
#if ANDROID
                    h.AddHandler<Shell, TabbarBadgeRenderer>();
#endif
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}