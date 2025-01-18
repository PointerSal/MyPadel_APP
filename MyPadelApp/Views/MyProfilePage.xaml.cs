using LocalizationResourceManager.Maui;

namespace MyPadelApp.Views;

public partial class MyProfilePage : ContentPage
{
    ILocalizationResourceManager localizationResourceManager = App.Current.Handler.MauiContext.Services.GetService<ILocalizationResourceManager>();
	public MyProfilePage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelectionPage(localizationResourceManager));
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ProfileInformationPage());
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new FITCardPage());
    }

    private async void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new MedicalCertificatePage());
    }

    private async void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PaymentMethodPage());
    }

    private async void TapGestureRecognizer_Tapped_4(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AccountDeletionPage());
    }
}