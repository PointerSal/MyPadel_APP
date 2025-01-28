using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel registerViewModel)
	{
		InitializeComponent();
        BindingContext = registerViewModel;
    }
    private void passwordImage_Clicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordImage.Source = passwordEntry.IsPassword ? "closeeye" : "openeye";
    }
}