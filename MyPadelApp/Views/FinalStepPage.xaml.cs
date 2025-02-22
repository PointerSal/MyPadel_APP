using MyPadelApp.Resources.Languages;

namespace MyPadelApp.Views;

public partial class FinalStepPage : ContentPage
{
	public FinalStepPage()
	{
		InitializeComponent();
	}
    private void OnCheckedBoxClicked(object sender, TappedEventArgs e)
    {
        image1.IsVisible = !image1.IsVisible;
    }
    private void OnFirstRadioClicked(object sender, TappedEventArgs e)
    {
        InnerBorder1.IsVisible = true;
        InnerBorder2.IsVisible = !InnerBorder1.IsVisible;
    }
    private void OnSecondRadioClicked(object sender, TappedEventArgs e)
    {
        InnerBorder2.IsVisible = true;
        InnerBorder1.IsVisible = !InnerBorder2.IsVisible;
    }

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        try
        {
            if (!image1.IsVisible)
                await Shell.Current.DisplayAlert(AppResources.Error, AppResources.TermsAndConditionAgree, AppResources.OK);
            else if (InnerBorder1.IsVisible)
                await Shell.Current.GoToAsync("AlreadyFITCardPage");
            else if (InnerBorder2.IsVisible)
                await Shell.Current.GoToAsync("CreateFIFPage");
            else
                await Shell.Current.DisplayAlert(AppResources.Error, AppResources.OneSelected, AppResources.OK);
        }
        catch(Exception ex) { }
    }
}