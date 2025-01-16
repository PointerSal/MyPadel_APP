namespace MyPadelApp.Views;

public partial class BookedFieldPage : ContentPage
{
	public BookedFieldPage()
	{
		InitializeComponent();
	}

    private async void OnBookedClicked(object sender, EventArgs e)
    {
		await Navigation.PopToRootAsync(true);
    }
}