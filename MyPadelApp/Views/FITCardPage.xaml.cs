namespace MyPadelApp.Views;

public partial class FITCardPage : ContentPage
{
	public FITCardPage()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
    }
}