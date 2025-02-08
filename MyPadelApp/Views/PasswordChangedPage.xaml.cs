using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class PasswordChangedPage : ContentPage
{
	public PasswordChangedPage(PasswordChangedViewModel passwordChangedViewModel)
	{
		InitializeComponent();
        BindingContext = passwordChangedViewModel;
    }

    private void OnPasswordEyeClicked(object sender, TappedEventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;
        passwordImage.Source = passwordEntry.IsPassword ? "closeeye" : "openeye";
    }
    private void OnConfirmPasswordEyeClicked(object sender, TappedEventArgs e)
    {
        confirmpasswordEntry.IsPassword = !confirmpasswordEntry.IsPassword;
        confirmpasswordImage.Source = confirmpasswordEntry.IsPassword ? "closeeye" : "openeye";
    }
}