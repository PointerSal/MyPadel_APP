namespace MyPadelApp.Views;

public partial class ProfileInformationPage : ContentPage
{
	public ProfileInformationPage()
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ResetPasswordPage());
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CompanyInfoPage());
    }
}