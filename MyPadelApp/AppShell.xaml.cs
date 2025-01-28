using LocalizationResourceManager.Maui;
using MyPadelApp.Views;

namespace MyPadelApp
{
    public partial class AppShell : Shell
    {
        public AppShell(ILocalizationResourceManager localizationResourceManager)
        {
            InitializeComponent();
            Navigation.PushAsync(new SelectionPage(localizationResourceManager));

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegistrationResendOTPPage", typeof(RegistrationResendOTPPage));
            Routing.RegisterRoute("ResetPasswordOTPPage", typeof(ResetPasswordOTPPage));
            Routing.RegisterRoute("PasswordChangedPage", typeof(PasswordChangedPage));
            Routing.RegisterRoute("ResendEmailPage", typeof(ResendEmailPage));
            Routing.RegisterRoute("FinalStepPage", typeof(FinalStepPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        }
    }
}
