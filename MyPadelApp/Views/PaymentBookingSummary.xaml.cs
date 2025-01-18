namespace MyPadelApp.Views;

public partial class PaymentBookingSummary : ContentPage
{
	public PaymentBookingSummary()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PopToRootAsync(true);
    }
}