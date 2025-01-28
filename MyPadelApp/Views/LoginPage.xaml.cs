using LocalizationResourceManager.Maui;
using MyPadelApp.ViewModels;
using System.Globalization;

namespace MyPadelApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel loginViewModel)
	{
        InitializeComponent();
        BindingContext = loginViewModel;
    }

    private void passwordImage_Clicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordImage.Source = passwordEntry.IsPassword ? "closeeye" : "openeye";
    }
}