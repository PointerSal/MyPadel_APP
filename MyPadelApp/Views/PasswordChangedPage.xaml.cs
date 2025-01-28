using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class PasswordChangedPage : ContentPage
{
	public PasswordChangedPage(PasswordChangedViewModel passwordChangedViewModel)
	{
		InitializeComponent();
        BindingContext = passwordChangedViewModel;
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
}