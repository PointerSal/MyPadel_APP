namespace MyPadelApp.Views;

public partial class MedicalCertificatePage : ContentPage
{
	public MedicalCertificatePage()
	{
		InitializeComponent();
	}
    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync(true);
    }
}