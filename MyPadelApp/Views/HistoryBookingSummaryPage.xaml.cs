namespace MyPadelApp.Views;

public partial class HistoryBookingSummaryPage : ContentPage
{
	public HistoryBookingSummaryPage()
	{
		InitializeComponent();
	}

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CancelBookingPage());
    }
}