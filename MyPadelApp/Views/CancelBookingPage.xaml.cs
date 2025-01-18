namespace MyPadelApp.Views;

public partial class CancelBookingPage : ContentPage
{
	public CancelBookingPage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
    }
}