namespace MyPadelApp.Views;

public partial class PasswordChangedPage : ContentPage
{
	public PasswordChangedPage()
	{
		InitializeComponent();
	}

    private void OnPasswordEyeClicked(object sender, EventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordImage.Source = passwordEntry.IsPassword ? "closeeye" : "openeye";
    }

    private void OnConfirmPasswordEyeClicked(object sender, EventArgs e)
    {
        confirmpasswordEntry.IsPassword = !confirmpasswordEntry.IsPassword;
        confirmpasswordImage.Source = confirmpasswordEntry.IsPassword ? "closeeye" : "openeye";
    }

    private async void OnPasswordChange(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }
}