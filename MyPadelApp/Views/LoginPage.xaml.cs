using LocalizationResourceManager.Maui;
using System.Globalization;

namespace MyPadelApp.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
	{
        InitializeComponent();
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private void passwordImage_Clicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordImage.Source = passwordEntry.IsPassword ? "closeeye" : "openeye";
    }

    private async void OnResetPasswordClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordOTPPage());
    }

}