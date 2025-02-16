using MyPadelApp.ViewModels;

namespace MyPadelApp.Views;

public partial class AccountDeletionPage : ContentPage
{
	public AccountDeletionPage(AccountDeletionViewModel accountDeletionViewModel)
	{
		InitializeComponent();
        BindingContext = accountDeletionViewModel;
    }
    private void OnCurrentPasswordEyeClicked(object sender, EventArgs e)
    {
        try
        {
            currentpasswordEntry.IsPassword = !currentpasswordEntry.IsPassword;
            currentpasswordImage.Source = currentpasswordEntry.IsPassword ? "closeeye" : "openeye";
        }
        catch { }
    }
}