namespace MyPadelApp.Views;

public partial class ResendEmailPage : ContentPage
{
	public ResendEmailPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(4000);
        await Navigation.PushAsync(new RegistrationResendOTPPage());
    }
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
}