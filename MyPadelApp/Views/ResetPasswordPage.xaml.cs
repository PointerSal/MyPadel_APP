using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class ResetPasswordPage : ContentPage
{
	public ResetPasswordPage(ResetPasswordViewModel resetPasswordViewModel)
	{
		InitializeComponent();
        BindingContext = resetPasswordViewModel;
	}
    private void OnCurrentPasswordEyeClicked(object sender, EventArgs e)
    {
        currentpasswordEntry.IsPassword = !currentpasswordEntry.IsPassword;
        currentpasswordImage.Source = currentpasswordEntry.IsPassword ? "closeeye" : "openeye";
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