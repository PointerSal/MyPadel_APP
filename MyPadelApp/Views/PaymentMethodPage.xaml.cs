namespace MyPadelApp.Views;

public partial class PaymentMethodPage : ContentPage
{
	public PaymentMethodPage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
    }
}