using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class ResendEmailPage : ContentPage
{
	public ResendEmailPage(ResendEmailViewModel resendEmailViewModel)
	{
		InitializeComponent();
        BindingContext = resendEmailViewModel;
    }
}