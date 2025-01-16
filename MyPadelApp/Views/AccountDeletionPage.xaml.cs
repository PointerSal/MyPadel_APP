namespace MyPadelApp.Views;

public partial class AccountDeletionPage : ContentPage
{
	public AccountDeletionPage()
	{
		InitializeComponent();
	}
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
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