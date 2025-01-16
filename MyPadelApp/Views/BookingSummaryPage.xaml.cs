namespace MyPadelApp.Views;

public partial class BookingSummaryPage : ContentPage
{
	public BookingSummaryPage()
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(true);
    }
    private void OnBorderTapped(object sender, TappedEventArgs e)
    {
        min60Border.BackgroundColor = Colors.Transparent;
        min90Border.BackgroundColor = Colors.Transparent;
        min120Border.BackgroundColor = Colors.Transparent;

        if (sender == min60Border)
            min60Border.BackgroundColor = Color.FromArgb("#a4b0e7");
        else if (sender == min90Border)
            min90Border.BackgroundColor = Color.FromArgb("#a4b0e7");
        else if (sender == min120Border)
            min120Border.BackgroundColor = Color.FromArgb("#a4b0e7");
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PaymentScreenPage(true));
    }
}