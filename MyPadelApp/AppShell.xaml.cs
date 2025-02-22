using LocalizationResourceManager.Maui;
using MyPadelApp.Views;

namespace MyPadelApp
{
    public partial class AppShell : Shell
    {
        public AppShell(ILocalizationResourceManager localizationResourceManager)
        {
            InitializeComponent();

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegistrationResendOTPPage", typeof(RegistrationResendOTPPage));
            Routing.RegisterRoute("ResetPasswordOTPPage", typeof(ResetPasswordOTPPage));
            Routing.RegisterRoute("PasswordChangedPage", typeof(PasswordChangedPage));
            Routing.RegisterRoute("ResendEmailPage", typeof(ResendEmailPage));
            Routing.RegisterRoute("FinalStepPage", typeof(FinalStepPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("AlreadyFITCardPage", typeof(AlreadyFITCardPage));
            Routing.RegisterRoute("BookedFieldPage", typeof(BookedFieldPage));
            Routing.RegisterRoute("CreateFIFPage", typeof(CreateFIFPage));
            Routing.RegisterRoute("ResetPasswordPage", typeof(ResetPasswordPage));
            Routing.RegisterRoute("ProfileInformationPage", typeof(ProfileInformationPage));
            Routing.RegisterRoute("CompanyInfoPage", typeof(CompanyInfoPage));
            Routing.RegisterRoute("AccountDeletionPage", typeof(AccountDeletionPage));
            Routing.RegisterRoute("PaymentMethodPage", typeof(PaymentMethodPage));   
            Routing.RegisterRoute("FITCardPage", typeof(FITCardPage));   
            Routing.RegisterRoute("MedicalCertificatePage", typeof(MedicalCertificatePage));   
            Routing.RegisterRoute("HistoryBookingSummaryPage", typeof(HistoryBookingSummaryPage));   
            Routing.RegisterRoute("CancelBookingPage", typeof(CancelBookingPage));   
            Routing.RegisterRoute("BookingSummaryPage", typeof(BookingSummaryPage));   
            Routing.RegisterRoute("PaymentScreenPage", typeof(PaymentScreenPage));   
            Routing.RegisterRoute("PaymentBookingSummary", typeof(PaymentBookingSummary));   
            Routing.RegisterRoute("FITPaymentPage", typeof(FITPaymentPage));   
        }
    }
}
