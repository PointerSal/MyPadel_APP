﻿using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using MyPadelApp.Resources.Languages;
using MyPadelApp.ViewModels;
using MyPadelApp.Views;

namespace MyPadelApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    handler.PlatformView.TextCursorDrawable.SetTint(Android.Graphics.Color.White);
#elif __IOS__                 
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.White;                 
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.Line; 
#endif
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Q)
                    handler.PlatformView.TextCursorDrawable.SetTint(Android.Graphics.Color.White);
#elif __IOS__                 
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.White;                 
                    //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.Line; 
#endif
            });

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalizationResourceManager(settings =>
                {
                    settings.RestoreLatestCulture(true);
                    settings.AddResource(AppResources.ResourceManager);
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ResetPasswordOTPViewModel>();
            builder.Services.AddTransient<ResetPasswordOTPPage>();
            builder.Services.AddTransient<PasswordChangedViewModel>();
            builder.Services.AddTransient<PasswordChangedPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<ResendEmailViewModel>();
            builder.Services.AddTransient<ResendEmailPage>();
            builder.Services.AddTransient<RegistrationResendOTPViewModel>();
            builder.Services.AddTransient<RegistrationResendOTPPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
