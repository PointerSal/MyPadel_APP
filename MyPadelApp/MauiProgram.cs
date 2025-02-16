using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using MyPadelApp.Resources.Languages;
using MyPadelApp.Services.AuthServices;
using MyPadelApp.Services.BookingServices;
using MyPadelApp.Services.HttpClientServices;
using MyPadelApp.Services.MembershipUserServices;
using MyPadelApp.ViewModels;
using MyPadelApp.Views;

namespace MyPadelApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
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

            builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
            builder.Services.AddSingleton<IMembershipUserService, MembershipUserService>();
            builder.Services.AddSingleton<IAuthServices, AuthServices>();
            builder.Services.AddSingleton<IBookingService, BookingService>();

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
            builder.Services.AddTransient<AlreadyFITCardViewModel>();
            builder.Services.AddTransient<AlreadyFITCardPage>();
            builder.Services.AddTransient<BookedFieldPage>();
            builder.Services.AddTransient<CreateFIFViewModel>();
            builder.Services.AddTransient<CreateFIFPage>();
            builder.Services.AddTransient<MyProfileViewModel>();
            builder.Services.AddTransient<MyProfilePage>();
            builder.Services.AddTransient<ProfileInformationViewModel>();
            builder.Services.AddTransient<ProfileInformationPage>();
            builder.Services.AddTransient<ResetPasswordViewModel>();
            builder.Services.AddTransient<ResetPasswordPage>();
            builder.Services.AddTransient<AccountDeletionViewModel>();
            builder.Services.AddTransient<AccountDeletionPage>();
            builder.Services.AddTransient<PaymentMethodPage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<MedicalCertificatePage>();
            builder.Services.AddTransient<MedicalCertificateViewModel>();
            builder.Services.AddTransient<FITCardPage>();
            builder.Services.AddTransient<FITCardViewModel>();
            builder.Services.AddTransient<MatchHistoryViewModel>();
            builder.Services.AddTransient<MatchHistoryPage>();
            builder.Services.AddTransient<HistoryBookingSummaryViewModel>();
            builder.Services.AddTransient<HistoryBookingSummaryPage>();
            builder.Services.AddTransient<CancelBookingViewModel>();
            builder.Services.AddTransient<CancelBookingPage>();
            builder.Services.AddTransient<BookingSummaryPage>();
            builder.Services.AddTransient<BookingSummaryViewModel>();
            builder.Services.AddTransient<PaymentBookingSummary>();
            builder.Services.AddTransient<PaymentBookingSummaryViewModel>();
            builder.Services.AddTransient<PaymentScreenPage>();
            builder.Services.AddTransient<PaymentScreenViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
